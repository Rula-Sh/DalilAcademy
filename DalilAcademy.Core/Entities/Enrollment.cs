using DalilAcademy.Core.Entites;

namespace DalilAcademy.Core.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public DateTime EnrolledtAt { get; set; }
    }
}
