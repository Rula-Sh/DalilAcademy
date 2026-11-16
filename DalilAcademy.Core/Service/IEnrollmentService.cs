using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Service
{
    public interface IEnrollmentService
    {
        Task<List<EnrollmentResponseDto>> GetAllAsync();
        Task<PageResult<EnrollmentResponseDto>> GetPageAsync(int page, int pageSize);
        Task<PageResult<StudentEnrollmentsResponseDto>> GetStudentEnrollmentsAsync(int userId, int page, int pageSize);
        Task<EnrollmentResponseDto?> GetByIdAsync(int id);
        Task<Enrollment?> GetByStudentAndCourseAsync(int studentId, int courseId);
        Task<EnrollmentResponseDto> EnrollAsync(CreateEnrollmentDto dto);
        Task ChangeAsync(int id,UpdateEnrollmentDto dto);
        Task DeleteAsync(int id);
    }
}