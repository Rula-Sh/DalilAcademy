using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Service
{
    public interface IInstructorService
    {
        Task<List<InstructorResponseDto>> GetAllAsync();
        Task<PageResult<InstructorResponseDto>> GetPageAsync(int page, int pageSize);
        Task<PageResult<CourseResponseDto>> GetCoursesInPageAsync(int id, int page, int pageSize);
        Task<InstructorResponseDto?> GetByIdAsync(int id);
        Task<InstructorResponseDto> CreateAsync(CreateInstructorDto dto, string hashPass);
        Task<User> RegisterAsync(CreateInstructorDto dto, string hashPass);
        Task UpdateAsync(int id, UpdateInstructorDto Instructor);
        Task DeleteAsync(int id);
    }
}
