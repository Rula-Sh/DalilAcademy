using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DalilAcademy.Infra.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) // this searches for every class in the infra project and implements the IEntityTypeConfiguration and executes it on it own
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSeeding((context, _) =>
            {
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
