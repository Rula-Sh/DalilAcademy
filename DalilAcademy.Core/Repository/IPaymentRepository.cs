using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task<Payment?> GetLatestCreatedByEnrollmentIdAsync(int enrollmentId);
    }
}
