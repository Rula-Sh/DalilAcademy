using DalilAcademy.Core.Entites;

namespace DalilAcademy.Core.Repository
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<Course> CreateAsync(Course course);
        Task UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
    }
}
