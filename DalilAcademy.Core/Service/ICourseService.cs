using DalilAcademy.Core.Entites;

namespace DalilAcademy.Core.Service
{
    public interface ICourseService
    {
        List<Course> GetAll();
        Course GetById(int id);
        Course Create(Course course);
        Course Update(Course course);
        Course Delete(int id);
    }
}
