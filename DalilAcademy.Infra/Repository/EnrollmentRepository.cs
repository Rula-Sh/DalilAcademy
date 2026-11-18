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
            return await db.Enrollments.Select(e => new EnrollmentResponseDto(e.Id, e.Student!.FullName, e.Course!.Title, e.EnrolledAt, e.Status.ToString())).ToListAsync();
        }
        public async Task<PageResult<EnrollmentResponseDto>> GetPageAsync(int page, int pageSize)
        {
            var count = await db.Enrollments.CountAsync();
            var items = await db.Enrollments
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EnrollmentResponseDto(e.Id, e.Student!.FullName, e.Course!.Title, e.EnrolledAt, e.Status.ToString()))
                .ToListAsync();
            return new PageResult<EnrollmentResponseDto>(items, page, pageSize, count);
        }
        public async Task<PageResult<StudentEnrollmentsResponseDto>> GetStudentEnrollmentsAsync(int studentId, int page, int pageSize)
        {
            var query = db.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == studentId && e.Status == EnrollmentStatus.Paid);

            var count = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new StudentEnrollmentsResponseDto(e.Course!.Title, e.EnrolledAt))
                .ToListAsync();
            return new PageResult<StudentEnrollmentsResponseDto>(items, page, pageSize, count);
        }
        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await db.Enrollments.Include(e => e.Student).Include(e => e.Course).FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<List<PaymentResponseDto>> GetPaymentsAsync(int id)
        {
            return await db.Payments
                .Where(p => p.EnrollmentId == id)
                .OrderByDescending(p => p.PaidAt)
                .Select(p => new PaymentResponseDto(p.Id, p.PayPalOrderId, p.Amount, p.Currency, p.Status.ToString(), p.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                                                    p.PaidAt != null ? p.PaidAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "Didn't Pay"))
                .ToListAsync();
        }
        public async Task<int> GetEnrolledCountByCourseIdAsync(int id)
        {
            return await db.Enrollments.Where(e => e.Course!.Id == id && e.Status == EnrollmentStatus.Paid).CountAsync();
        }
        public async Task<Enrollment?> GetByStudentAndCourseAsync(int studentId, int courseId)
        {
            return await db.Enrollments.FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
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
        public async Task ChangeAsync(Enrollment enrollment)
        {
            try
            {
                db.Enrollments.Update(enrollment);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("The student is already enrolled to this course.");
            }
        }
        public async Task<bool> CancelAsync(int id)
        {
            var enrollment = await db.Enrollments.FindAsync(id);
            if (enrollment is null)
                return false;

            db.Enrollments.Remove(enrollment);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
