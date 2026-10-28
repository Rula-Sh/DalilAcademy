using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DalilAcademy.Infra.Repository
{
    public class StudentRepository(AppDbContext db) : IStudentRepository
    {

        public async Task<List<Student>> GetAllAsync()
        {
            return await db.Students.ToListAsync();
        }
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await db.Students.FindAsync(id);
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
                throw new ArgumentException("A student with this email already exists.");
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
                throw new ArgumentException("A student with this email already exists.");
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
