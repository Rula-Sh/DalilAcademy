using DalilAcademy.Core.DTOs;

namespace DalilAcademy.Core.Service
{
    public interface ICourseService
    {
        Task<List<CourseResponseDto>> GetAllAsync();
        Task<CourseResponseDto?> GetByIdAsync(int id);
        Task<CourseResponseDto> CreateAsync(CreateCourseDto course);
        Task UpdateAsync(int id, Course course);
        Task DeleteAsync(int id);
    }
}
