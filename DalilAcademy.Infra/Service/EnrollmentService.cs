using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using FluentValidation;

namespace DalilAcademy.Infra.Service
{
    public class EnrollmentService(IStudentRepository students, ICourseRepository courses, IEnrollmentRepository enrollments, IValidator<CreateEnrollmentDto> validator) : IEnrollmentService
    {
        public Task<List<EnrollmentResponseDto>> GetAllAsync()
        {
            return enrollments.GetAllAsync();
        }
        public Task<PageResult<EnrollmentResponseDto>> GetPageAsync(int page, int pageSize)
        {
            return enrollments.GetPageAsync(page, pageSize);
        }

        public async Task<PageResult<StudentEnrollmentsResponseDto>> GetStudentEnrollmentsAsync(int userId, int page, int pageSize)
        {
            var student = await students.GetByUserIdAsync(userId);

            if (student is null)
                throw new NotFoundException($"Student with UserId {userId} was not found.");

            return await enrollments.GetStudentEnrollmentsAsync(student!.Id, page, pageSize);
        }

        public async Task<EnrollmentResponseDto?> GetByIdAsync(int id)
        {
            var enrollment = await enrollments.GetByIdAsync(id) ?? throw new NotFoundException($"Enrollment with ID {id} was not found");
            return enrollment?.ToDto();
        }
        public async Task<EnrollmentResponseDto> EnrollAsync(int studentId, int courseId)
        {
            if (await students.GetByIdAsync(studentId) is null)
                throw new NotFoundException("Student not found.");

            if (await courses.GetByIdAsync(courseId) is null)
                throw new NotFoundException("Course not found.");

            var dto = new CreateEnrollmentDto(studentId, courseId);

            await validator.ValidateAndThrowAsync(dto);

            var enrollment = dto.ToEntity();
            var created = await enrollments.EnrollAsync(enrollment); // unique index: already enrolled → 409

            return created.ToDto();
        }
    }
    // Three exceptions, three status codes: 404, 404, and 409 Conflict. The 409 comes from the unique index, caught in the repository
}
