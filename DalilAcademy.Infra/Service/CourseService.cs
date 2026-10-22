using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;

namespace DalilAcademy.Infrastructure.Service
{
    public class CourseService(ICourseRepository courseRepository) : ICourseService
    {
        public List<Course> GetAll() {
            return courseRepository.GetAll();
        }
        public Course GetById(int id)
        {
            return courseRepository.GetById(id);
        }
        public Course Create(Course course)
        {
            return courseRepository.Create(course);
        }
        public Course Update(Course course)
        {
            return courseRepository.Update(course);
        }
        public Course Delete(int id)
        {
            return courseRepository.Delete(id);
        }
    }
}
