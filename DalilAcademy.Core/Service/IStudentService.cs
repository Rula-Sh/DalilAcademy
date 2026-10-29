using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Service
{
    public interface IStudentService
    {
        Task<List<StudentResponseDto>> GetAllAsync();
        Task<StudentResponseDto?> GetByIdAsync(int id);
        Task<StudentResponseDto> CreateAsync(CreateStudentDto student);
        Task UpdateAsync(int id, UpdateStudentDto student);
        Task DeleteAsync(int id);
    }
}
