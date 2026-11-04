using DalilAcademy.Core.DTOs;

namespace DalilAcademy.Core.Service
{
    public interface IEnrollmentService
    {
        Task<List<EnrollmentResponseDto>> GetAllAsync();
        Task<PageResult<EnrollmentResponseDto>> GetPageAsync(int page, int pageSize);
        Task<EnrollmentResponseDto?> GetByIdAsync(int id);
        Task<EnrollmentResponseDto> EnrollAsync(int studentId, int courseId);
    }
}
