using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface IInstructorRepository
    {
        Task<List<InstructorResponseDto>> GetAllAsync();
        Task<PageResult<InstructorResponseDto>> GetPageAsync(int page, int pageSize);
        Task<PageResult<CourseResponseDto>> GetCoursesInPageAsync(int id, int page, int pageSize);
        Task<Instructor?> GetByIdAsync(int id);
        Task<Instructor?> GetByUserIdAsync(int id);
        Task<Instructor> CreateAsync(Instructor Instructor);
        Task UpdateAsync(Instructor Instructor);
        Task<bool> DeleteAsync(int id);
    }
}
