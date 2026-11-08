using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DalilAcademy.Infra.Repository
{
    public class StudentRepository(AppDbContext db) : IStudentRepository
    {
        public async Task<List<StudentResponseDto>> GetAllAsync()
        {
            return await db.Students.Select(s => new StudentResponseDto(s.Id, s.FullName, s.Email, s.Level)).ToListAsync();
        }
        public async Task<PageResult<StudentResponseDto>> GetPageAsync(int page, int pageSize)
        {
            var count = await db.Students.CountAsync();
            var items = await db.Students
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new StudentResponseDto(s.Id, s.FullName, s.Email, s.Level))
                .ToListAsync();
            return new PageResult<StudentResponseDto>(items, page, pageSize, count);
        }
        public async Task<PageResult<CourseResponseDto>> GetCoursesInPageAsync(int id, int page, int pageSize)
        {
            var count = await db.Students.CountAsync();
            var items = await db.Enrollments
                .OrderBy(e => e.CourseId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Where(e => e.StudentId == id)
                .Select(e => new CourseResponseDto(e.Course!.Id, e.Course.Title, e.Course.Price, e.Course.Instructor!.FullName, e.Course.ImageUrl))
                .ToListAsync();
            return new PageResult<CourseResponseDto>(items, page, pageSize, count);
        }
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await db.Students.FindAsync(id);
        }
        public async Task<Student?> GetByUserIdAsync(int id)
        {
            var student = await db.Students.FirstOrDefaultAsync(s => s.UserId == id);

            return student;
        }
        public async Task<Student> CreateAsync(Student student)
        {
            try
            {
                db.Students.Add(student);
                await db.SaveChangesAsync();
                return student;
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("A student with this email already exists.");
            }
        }
        public async Task UpdateAsync(Student student)
        {
            try
            {
                db.Students.Update(student);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("A student with this email already exists.");
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var student = await db.Students.FindAsync(id);
            if (student is null)
                return false;

            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
