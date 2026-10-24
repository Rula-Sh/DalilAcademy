using DalilAcademy.Core.Entites;

namespace DalilAcademy.Core.Entities
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<Course> Courses { get; set; } = [];
    }
}
