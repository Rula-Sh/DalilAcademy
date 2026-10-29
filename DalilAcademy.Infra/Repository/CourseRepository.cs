using DalilAcademy.Core.Entites;
using DalilAcademy.Core.Exceptions;
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
            //return await db.Courses.ToListAsync(); // ToListAsync does not exist in DbSet, so i need to import the extention: Microsoft.EntityFrameworkCore; to remove the error from ToListAsync
            return await db.Courses.Include(c=> c.Instructor).ToListAsync(); // added Include to get the instructor obj with the course (EF does not return the related tables by default unless we requested them)
            
        }
        public async Task<Course?> GetByIdAsync(int id)
        {
            return await db.Courses.Include(c => c.Instructor).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Course> CreateAsync(Course course)
        {
            try
            {
                db.Courses.Add(course); // insert
                await db.SaveChangesAsync(); // run INSERT to DB, and returns int (rows affected)
                // the EF preserves a file for each entity i deal with, the Entry() opens that file and helps us get the data, use 
                await db.Entry(course).Reference(c => c.Instructor).LoadAsync(); // this gets us the instructor (single) of the course from the DB, if i wanted to get the students (plural) i should use Collection()
                return course;
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("A course with this title already exists.");
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
