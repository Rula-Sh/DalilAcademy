using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface IEnrollmentRepository
    {
        Task<List<EnrollmentResponseDto>> GetAllAsync();
        Task<PageResult<EnrollmentResponseDto>> GetPageAsync(int page, int pageSize);
        Task<Enrollment?> GetByIdAsync(int id);
        Task<Enrollment> EnrollAsync(Enrollment enrollment);
    }
}
