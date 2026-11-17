using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DalilAcademy.Infra.Repository
{
    public class InstructorRepository(AppDbContext db) : IInstructorRepository
    {
        public async Task<List<InstructorResponseDto>> GetAllAsync()
        {
            return await db.Instructors.Select(s => new InstructorResponseDto(s.Id, s.FullName, s.User.Email)).ToListAsync();
        }
        public async Task<PageResult<InstructorResponseDto>> GetPageAsync(int page, int pageSize)
        {
            var count = await db.Instructors.CountAsync();
            var items = await db.Instructors
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new InstructorResponseDto(s.Id, s.FullName, s.User.Email))
                .ToListAsync();
            return new PageResult<InstructorResponseDto>(items, page, pageSize, count);
        }
        public async Task<PageResult<CourseResponseDto>> GetCoursesInPageAsync(int id, int page, int pageSize)
        {
            var count = await db.Courses.Where(c => c.InstructorId == id).CountAsync();
            var items = await db.Courses
                .Where(c => c.InstructorId == id)
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CourseResponseDto(c.Id, c.Title, c.Price, c.Capacity, c.Instructor!.FullName, c.ImageUrl))
                .ToListAsync();
            return new PageResult<CourseResponseDto>(items, page, pageSize, count);
        }
        public async Task<Instructor?> GetByIdAsync(int id)
        {
            return await db.Instructors.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Instructor?> GetByUserIdAsync(int id)
        {
            var Instructor = await db.Instructors.FirstOrDefaultAsync(s => s.UserId == id);

            return Instructor;
        }
        public async Task<Instructor> CreateAsync(Instructor Instructor)
        {
            try
            {
                db.Instructors.Add(Instructor);
                await db.SaveChangesAsync();
                return Instructor;
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("A user with this email already exists.");
            }
        }
        public async Task UpdateAsync(Instructor Instructor)
        {
            try
            {
                db.Instructors.Update(Instructor);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("A user with this email already exists.");
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var Instructor = await db.Instructors.FindAsync(id);
            if (Instructor is null)
                return false;

            db.Instructors.Remove(Instructor);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
