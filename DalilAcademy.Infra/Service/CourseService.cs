using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;

namespace DalilAcademy.Infra.Service
{
    public class CourseService(ICourseRepository courseRepository) : ICourseService
    {
        public Task<List<Course>> GetAllAsync()
        {
            return courseRepository.GetAllAsync();
        }
        public Task<Course?> GetByIdAsync(int id)
        {
            return courseRepository.GetByIdAsync(id);
        }
        public Task<Course> CreateAsync(Course course)
        {
            if (course.Price < 0)
                throw new ArgumentException("Price cannot be negative");

            return courseRepository.CreateAsync(course);
        }
        public async Task<bool> UpdateAsync(int id, Course input)
        {
            var course = await courseRepository.GetByIdAsync(id);
            if (course is null)
                return false;

            course.Title = input.Title;
            course.Price = input.Price;

            await courseRepository.UpdateAsync(course);
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await courseRepository.DeleteAsync(id);
        }
    }
}
