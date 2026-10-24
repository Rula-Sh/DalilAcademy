using DalilAcademy.Core.Entites;

namespace DalilAcademy.Core.Repository
{
    public interface ICourseRepository
    {
        List<Course> GetAll();
        Course GetById(int id);
        Course Create(Course course);
        Course Update(Course course);
        Course Delete(int id);
    }
}
