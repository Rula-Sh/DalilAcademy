using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface IStudentRepository
    {
        Task<List<StudentResponseDto>> GetAllAsync();
        Task<PageResult<StudentResponseDto>> GetPageAsync(int page, int pageSize);
        Task<PageResult<CourseResponseDto>> GetCoursesInPageAsync(int id, int page, int pageSize);
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByUserIdAsync(int id);
        Task<Student> CreateAsync(Student student);
        Task UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
    }
}
