using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DalilAcademy.Infra.Repository
{
    public class CourseRepository(AppDbContext db) : ICourseRepository
    {
        public async Task<int> GetCountAsync()
        {
            return await db.Courses.CountAsync();
        }
        public async Task<List<CourseResponseDto>> GetAllAsync()
        {
            /*
            // an older & bad approach... 100 courses will run 1 query to get all courses + another 100 to get the instructor for each course (a single request sends multiple queries, when it should send 1 or 2)
            var courses = await db.Courses.ToListAsync();
            foreach (var c in courses)
                c.Instructor = await db.Instructors.FirstOrDefaultAsync(i => i.Id == c.InstructorId);

            return courses;
            */

            //return await db.Courses.ToListAsync(); // ToListAsync does not exist in DbSet, so i need to import the extention: Microsoft.EntityFrameworkCore; to remove the error from ToListAsync
            //return await db.Courses.Include(c => c.Instructor).ToListAsync(); // added Include to get the instructor obj with the course (EF does not return the related tables by default unless we requested them)

            //var count = await db.Courses.CountAsync(); // if i wanted to get the number of courses, this is the best approach, to count it from the DB, rather than counting them using C#

            // Include is best used for updates and business logic, While Select is for GET endpoints
            return await db.Courses.Select(c => new CourseResponseDto(c.Id, c.Title, c.Price, c.Instructor!.FullName)).ToListAsync(); // updated the function return type to CourseResponseDto
        }
        public async Task<PageResult<CourseResponseDto>> GetPageAsync(int page, int pageSize)
        {
            var count = await db.Courses.CountAsync();
            var items = await db.Courses
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CourseResponseDto(c.Id, c.Title, c.Price, c.Instructor!.FullName))
                .ToListAsync();
            return new PageResult<CourseResponseDto>(items, page, pageSize, count);
        }
        public async Task<PageResult<CourseResponseDto>> GetByPricePageAsync(int min, int max, int page, int pageSize)
        {
            var count = await db.Courses.CountAsync();
            var items = await db.Courses
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Where(c => c.Price >= min && c.Price <= max)
                .Select(c => new CourseResponseDto(c.Id, c.Title, c.Price, c.Instructor!.FullName))
                .ToListAsync();
            return new PageResult<CourseResponseDto>(items, page, pageSize, count);
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
