using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Entites
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
        public List<Enrollment> Enrollments { get; set; } = [];

    }
}
