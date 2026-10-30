using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface IEnrollmentRepository
    {
        Task<List<EnrollmentResponseDto>> GetAllAsync();
        Task<Enrollment?> GetByIdAsync(int id);
        Task<Enrollment> EnrollAsync(Enrollment enrollment);
    }
}
