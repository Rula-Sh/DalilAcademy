using DalilAcademy.Core.DTOs;

namespace DalilAcademy.Core.Service
{
    public interface IPaymentService
    {
        Task<PaymentOrderResponseDto> PayAsync(int enrollmentId);
        Task<EnrollmentResponseDto> CaptureAsync(int enrollmentId);
    }
}
