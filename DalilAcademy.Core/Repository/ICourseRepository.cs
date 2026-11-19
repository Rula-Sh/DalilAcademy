using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface ICourseRepository
    {
        Task<int> GetCountAsync();
        Task<List<CourseResponseDto>> GetAllAsync();
        Task<List<CourseWithEnrollmentsDto>> GetAllWithEnrollmentsAsync();
        Task<List<StudentResponseDto>> GetPaidStudentsAsync(int id);
        Task<PageResult<CourseResponseDto>> GetPageAsync(int page, int pageSize);
        Task<PageResult<CourseResponseDto>> GetByPricePageAsync(PriceFilterQueryDto query, int page, int pageSize);
        Task<PageResult<CourseResponseDto>> GetBySearchAsync(string search, int page, int pageSize);
        Task<Course?> GetByIdAsync(int id);
        Task<Course> CreateAsync(Course course);
        Task UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
    }
}
