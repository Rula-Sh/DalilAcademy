using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Service
{
    public interface IStudentService
    {
        Task<List<StudentResponseDto>> GetAllAsync();
        Task<PageResult<StudentResponseDto>> GetPageAsync(int page, int pageSize);
        Task<PageResult<CourseResponseDto>> GetCoursesInPageAsync(int id, int page, int pageSize);
        Task<StudentResponseDto?> GetByIdAsync(int id);
        Task<StudentResponseDto> CreateAsync(CreateStudentDto dto, string hashPass);
        Task<User> RegisterAsync(CreateStudentDto dto, string hashPass);
        Task UpdateAsync(int id, UpdateStudentDto student);
        Task DeleteAsync(int id);
    }
}
