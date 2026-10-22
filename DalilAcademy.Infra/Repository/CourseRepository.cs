using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Repository;
using System.Runtime.InteropServices;

namespace DalilAcademy.Infrastructure.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private static readonly List<Course> _courses = []; // static keeps the list alive
        public List<Course> GetAll()
        {
            return _courses;
        }
        public Course GetById(int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            return course;
        }
        public Course Create(Course course)
        {
            course.Id = _courses.Count == 0 ? 1 : _courses.Max(c => c.Id) + 1;
            _courses.Add(course);
            return course;
        }
        public Course Update(Course course)
        {
            var courseIndex = _courses.FindIndex(c => c.Id == course.Id);
            if (courseIndex == -1)
                return null;

            _courses[courseIndex] = course;
            return course;
        }
        public Course Delete(int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            if (course is null) return null;
            _courses.Remove(course);
            return course;
        }
    }
}
