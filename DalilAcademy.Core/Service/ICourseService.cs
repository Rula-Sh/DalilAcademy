using DalilAcademy.Core.DTOs;

namespace DalilAcademy.Core.Service
{
    public interface ICourseService
    {
        Task<int> GetCountAsync();
        Task<List<CourseResponseDto>> GetAllAsync();
        Task<List<CourseWithEnrollmentsDto>> GetAllWithEnrollmentsAsync();
        Task<PageResult<CourseResponseDto>> GetPageAsync(int page, int pageSize);
        Task<PageResult<CourseResponseDto>> GetByPricePageAsync(int min, int max, int page, int pageSize);
        Task<CourseResponseDto?> GetByIdAsync(int id);
        Task<CourseResponseDto> CreateAsync(CreateCourseDto course);
        Task UpdateAsync(int id, UpdateCourseDto course);
        Task DeleteAsync(int id);
    }
}
