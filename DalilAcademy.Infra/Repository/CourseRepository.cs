using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;

namespace DalilAcademy.Infra.Repository
{
    public class CourseRepository(AppDbContext db) : ICourseRepository
    {
        public List<Course> GetAll()
        {
            return db.Courses.ToList();
        }
        public Course GetById(int id)
        {
            return db.Courses.Find(id);
        }
        public Course Create(Course course)
        {
            db.Courses.Add(course);
            db.SaveChanges();
            return course;
        }
        public Course Update(Course course)
        {
            db.Courses.Update(course);
            db.SaveChanges();
            return course;
        }
        public Course Delete(int id)
        {
            var course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return course;
        }
    }
}
