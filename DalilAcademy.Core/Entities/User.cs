namespace DalilAcademy.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // never the real password
        public string Role { get; set; } = Roles.Student; // set role to Student by def (best to set the role with the least privileges)
        public bool RequireUpdatingPassword { get; set; } = false;
    }

    public static class Roles
    {
        public const string Admin = "Admin", Instructor = "Instructor", Student = "Student";
    }
}
