using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DalilAcademy.Infra.Repository
{
    public class EnrollmentRepository(AppDbContext db) : IEnrollmentRepository
    {
        public async Task<List<EnrollmentResponseDto>> GetAllAsync()
        {
            return await db.Enrollments.Select(e => new EnrollmentResponseDto(e.Id, e.Student!.FullName, e.Course!.Title, e.EnrolledAt)).ToListAsync();
        }
        public async Task<PageResult<EnrollmentResponseDto>> GetPageAsync(int page, int pageSize)
        {
            var count = await db.Enrollments.CountAsync();
            var items = await db.Enrollments
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EnrollmentResponseDto(e.Id, e.Student!.FullName, e.Course!.Title, e.EnrolledAt))
                .ToListAsync();
            return new PageResult<EnrollmentResponseDto>(items, page, pageSize, count);
        }
        public async Task<PageResult<StudentEnrollmentsResponseDto>> GetStudentEnrollmentsAsync(int studentId, int page, int pageSize)
        {
            var count= await db.Enrollments.Where(e=> e.StudentId == studentId).CountAsync();
            var items = await db.Enrollments
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Where(e => e.StudentId == studentId)
                .Select(e => new StudentEnrollmentsResponseDto(e.Course!.Title, e.EnrolledAt))
                .ToListAsync();
            return new PageResult<StudentEnrollmentsResponseDto>(items, page, pageSize, count);
        }
        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await db.Enrollments.Include(e => e.Student).Include(e => e.Course).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Enrollment> EnrollAsync(Enrollment enrollment)
        {
            try
            {
                db.Enrollments.Add(enrollment);
                await db.SaveChangesAsync();

                await db.Entry(enrollment).Reference(e => e.Student).LoadAsync();
                await db.Entry(enrollment).Reference(e => e.Course).LoadAsync();

                return enrollment;
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("The student is already enrolled to this course.");
            }
        }
    }
}
