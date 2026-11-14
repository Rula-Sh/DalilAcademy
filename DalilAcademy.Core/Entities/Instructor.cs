namespace DalilAcademy.Core.Entities
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email => User.Email;
        public List<Course> Courses { get; set; } = [];
        public int UserId { get; set; }
        public required User User { get; set; }
    }
}
