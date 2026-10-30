
using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace DalilAcademy.Infra.Repository
{
    public class EnrollmentRepository(AppDbContext db) : IEnrollmentRepository
    {
        public async Task<List<EnrollmentResponseDto>> GetAllAsync()
        {
            return await db.Enrollments.Select(e => new EnrollmentResponseDto(e.Id, e.Student!.FullName, e.Course!.Title, e.EnrolledtAt)).ToListAsync();
        }
        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await db.Enrollments.Include(e => e.Student).Include(e => e.Course).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Enrollment> EnrollAsync(Enrollment enrollment)
        {
            db.Enrollments.Add(enrollment);
            await db.SaveChangesAsync();

            await db.Entry(enrollment).Reference(e => e.Student).LoadAsync();
            await db.Entry(enrollment).Reference(e => e.Student).LoadAsync();

            return enrollment;
        }
    }
}
