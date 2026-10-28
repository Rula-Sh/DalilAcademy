using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DalilAcademy.Infra.Repository
{
    public class CourseRepository(AppDbContext db) : ICourseRepository
    {
        public async Task<List<Course>> GetAllAsync()
        {
            return await db.Courses.ToListAsync(); // ToListAsync does not exist in DbSet, so i need to import the exention: Microsoft.EntityFrameworkCore; to remove the error from ToListAsync
        }
        public async Task<Course?> GetByIdAsync(int id)
        {
            return await db.Courses.FindAsync(id);
        }
        public async Task<Course> CreateAsync(Course course)
        {
            try
            {
                db.Courses.Add(course); // insert
                await db.SaveChangesAsync(); // run  INSERT to DB, and returns int (rows affected)
                return course;
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ArgumentException("A course with this title already exists.");
            }
        }
        public async Task UpdateAsync(Course course)
        {
            try
            {
                db.Courses.Update(course);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ArgumentException("A course with this title already exists.");
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var course = await db.Courses.FindAsync(id);
            if (course is null)
                return false;

            db.Courses.Remove(course);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
