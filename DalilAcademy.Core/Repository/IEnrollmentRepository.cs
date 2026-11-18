using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface IEnrollmentRepository
    {
        Task<List<EnrollmentResponseDto>> GetAllAsync();
        Task<PageResult<EnrollmentResponseDto>> GetPageAsync(int page, int pageSize);
        Task<PageResult<StudentEnrollmentsResponseDto>> GetStudentEnrollmentsAsync(int studentId, int page, int pageSize);
        Task<Enrollment?> GetByIdAsync(int id);
        Task<Enrollment?> GetByStudentAndCourseAsync(int studentId, int courseId);
        Task<List<PaymentResponseDto>> GetPaymentsAsync(int id);
        Task<int> GetEnrolledCountByCourseIdAsync(int id);
        Task<Enrollment> EnrollAsync(Enrollment enrollment);
        Task ChangeAsync(Enrollment enrollment);
        Task<bool> CancelAsync(int id);
    }
}
