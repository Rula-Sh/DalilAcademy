using DalilAcademy.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DalilAcademy.Infra.Data
{
    // Options (provider + connection string) come from DI - check AddDbContext in Program.cs
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        // Tables
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // OnModelCreating Finds every IEntityTypeConfiguration<T> in this project and executes it on its own. If i forgot to implement IEntityTypeConfiguration<T>, the configurations will not be applied on the table
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Runs automatically on Update-Database
            optionsBuilder.UseSeeding((context, _) =>
            {
                if (!context.Set<Student>().Any()) // Seeds only once (works every time we update the database, without this line, it will add all the data again)
                {
                    var qasem = new Student { FullName = "Qasem Ali", Email = "qasemali@gmail.com", Level = 1 };
                    var basem = new Student { FullName = "Basem Amjad", Email = "basemamjad@gmail.com", Level = 2 };
                }

                if (!context.Set<Instructor>().Any())
                {
                    var ahmad = new Instructor { FullName = "Ahmad Ali" };
                    var raghad = new Instructor { FullName = "Raghad Amjad" };

                    if (!context.Set<Course>().Any())
                    {
                        context.Set<Course>().AddRange(
                            new Course { Title = "C# Fundamentals", Price = 500, Instructor = ahmad },
                            new Course { Title = "ASP.NET Core Web API", Price = 600, Instructor = raghad }
                        );
                    }
                }

                if (!context.Set<User>().Any())
                {
                    context.Set<User>().Add(new User
                    {
                        Email = "admin@dalil.jo",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        Role = Roles.Admin,
                        RequireUpdatingPassword = true
                    });

                    context.Set<User>().Add(new User
                    {
                        Email = "instructor@dalil.jo",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Instructor@123"),
                        Role = Roles.Instructor,
                        RequireUpdatingPassword = true
                    });

                    context.Set<User>().Add(new User
                    {
                        Email = "student@dalil.jo",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                        Role = Roles.Student,
                        RequireUpdatingPassword = true
                    });
                }
                context.SaveChanges();
            });
        }
    }
}
