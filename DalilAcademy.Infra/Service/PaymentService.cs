using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Payments;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using DalilAcademy.Infra.Payments;
using Microsoft.Extensions.Options;

namespace DalilAcademy.Infra.Service
{
    public class PaymentService(IPaymentRepository paymentRepo, ICourseRepository courses, IEnrollmentRepository enrollments, IPaymentGateway gateway, IOptions<PayPalSetting> options) : IPaymentService
    {
        readonly PayPalSetting paypal = options.Value;

        public async Task<PaymentOrderResponseDto> PayAsync(int enrollmentId)
        {
            var latestCreatedPayment = await paymentRepo.GetLatestCreatedByEnrollmentIdAsync(enrollmentId);
            if (latestCreatedPayment is not null)
            {
                if (latestCreatedPayment.CreatedAt.AddHours(paypal.ExpiresInHours) > DateTime.UtcNow)
                    throw new ConflictException("An active payment already exists. Call /capture or wait for it to expire.");

                latestCreatedPayment.Status = PaymentStatus.Failed;
                await paymentRepo.UpdateAsync(latestCreatedPayment);
            }

            var enrollment = await enrollments.GetByIdAsync(enrollmentId) ?? throw new NotFoundException("Enrollment not found.");
            var course = await courses.GetByIdAsync(enrollment.CourseId) ?? throw new NotFoundException("Course not found.");

            if (enrollment.Status == EnrollmentStatus.Paid)
                throw new ConflictException("Enrollment is already paid.");

            var amountUsd = Math.Round(course.Price * paypal.JODToUSDRate, 2);

            CreatePayPalOrderResponseDto payPalOrderResponse;
            try
            {
                payPalOrderResponse = await gateway.CreateOrderAsync(amountUsd);
            }
            catch (HttpRequestException)
            {
                throw new ServiceUnavailableException("PayPal is unreachable. Please try again later.");
            }

            var createdPayment = new Payment
            {
                EnrollmentId = enrollmentId,
                Amount = amountUsd,
                Currency = "USD",
                PayPalOrderId = payPalOrderResponse.OrderId,
                Status = PaymentStatus.Created,
                CreatedAt = DateTime.UtcNow
            };

            var payment = await paymentRepo.CreateAsync(createdPayment);

            return payment.ToDto(payPalOrderResponse);
        }

        public async Task<EnrollmentResponseDto> CaptureAsync(int enrollmentId)
        {
            var enrollment = await enrollments.GetByIdAsync(enrollmentId) ?? throw new NotFoundException("Enrollment not found.");

            if (enrollment.Status == EnrollmentStatus.Paid)
                throw new ConflictException("Enrollment is already paid.");

            var latestCreatedPayment = await paymentRepo.GetLatestCreatedByEnrollmentIdAsync(enrollmentId) ?? throw new BadRequestException("Call /pay first to create a createdPayment order before capturing.");

            if (latestCreatedPayment.CreatedAt.AddHours(paypal.ExpiresInHours) < DateTime.UtcNow)
            {
                latestCreatedPayment.Status = PaymentStatus.Failed;
                await paymentRepo.UpdateAsync(latestCreatedPayment);
                throw new PaymentFailedException("the createdPayment session has ended, call /pay again.");
            }

            bool captured;
            try
            {
                captured = await gateway.CaptureOrderAsync(latestCreatedPayment.PayPalOrderId);
            }
            catch (HttpRequestException)
            {
                throw new ServiceUnavailableException("PayPal is unreachable. Please try again later.");
            }

            if (!captured)
            {
                latestCreatedPayment.Status = PaymentStatus.Failed;
                await paymentRepo.UpdateAsync(latestCreatedPayment);
                throw new PaymentFailedException("Payment capture failed or not approved yet, call /pay again.");
            }

            latestCreatedPayment.Status = PaymentStatus.Completed;
            latestCreatedPayment.PaidAt = DateTime.UtcNow;
            await paymentRepo.UpdateAsync(latestCreatedPayment);

            enrollment.Status = EnrollmentStatus.Paid;
            await enrollments.ChangeAsync(enrollment);

            return enrollment.ToDto();
        }
    }
}
