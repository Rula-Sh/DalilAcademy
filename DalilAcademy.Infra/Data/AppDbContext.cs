using DalilAcademy.Core.Entites;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Finds every IEntityTypeConfiguration<T> in this project and executes it on its own
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Runs automatically on Update-Database
            optionsBuilder.UseSeeding((context, _) =>
            {
                if (context.Set<Student>().Any()) return;
                var Qasem = new Student { FullName = "Qasem Ali", Email = "qasemali@gmail.com", Level = 1 };
                var Basem = new Student { FullName = "Basem Amjad", Email = "basemamjad@gmail.com", Level = 2 };

                if (context.Set<Instructor>().Any()) return; // Seed only once (works every time we update the database, without this line, it will add all the data again)
                var ahmad = new Instructor { FullName = "Ahmad Ali" };
                var raghad = new Instructor { FullName = "Raghad Amjad" };

                context.Set<Course>().AddRange(
                    new Course { Title = "C# Fundamentals", Price = 500, Instructor = ahmad },
                    new Course { Title = "ASP.NET Core Web API", Price = 600, Instructor = raghad }
                );
                context.SaveChanges();
            });
        }

    }
}
