namespace DalilAcademy.Core.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public List<Payment> Payments { get; set; } = [];

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;
    }

    public enum EnrollmentStatus { Pending, Paid }
}
