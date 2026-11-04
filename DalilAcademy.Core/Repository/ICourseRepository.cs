using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface ICourseRepository
    {
        Task<int> GetCountAsync();
        Task<List<CourseResponseDto>> GetAllAsync();
        Task<PageResult<CourseResponseDto>> GetPageAsync(int page, int pageSize);
        Task<PageResult<CourseResponseDto>> GetByPricePageAsync(int min, int max, int page, int pageSize);
        Task<Course?> GetByIdAsync(int id);
        Task<Course> CreateAsync(Course course);
        Task UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
    }
}
