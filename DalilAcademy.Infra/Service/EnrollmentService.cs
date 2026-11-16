using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using FluentValidation;

namespace DalilAcademy.Infra.Service
{
    public class EnrollmentService(
            IStudentRepository students,
            ICourseRepository courses,
            IEnrollmentRepository enrollments,
            IValidator<CreateEnrollmentDto> createValidator,
            IValidator<UpdateEnrollmentDto> updateValidator
        ) : IEnrollmentService
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
            if (id < 1)
                throw new BadRequestException("Invalid Enrollement ID");

            var enrollment = await enrollments.GetByIdAsync(id) ?? throw new NotFoundException($"Enrollment with ID {id} was not found");
            return enrollment?.ToDto();
        }
        public async Task<Enrollment?> GetByStudentAndCourseAsync(int studentId, int courseId)
        {
            if (await students.GetByIdAsync(studentId) is null)
                throw new NotFoundException("Student not found.");

            if (await courses.GetByIdAsync(courseId) is null)
                throw new NotFoundException("Course not found.");

            return await enrollments.GetByStudentAndCourseAsync(studentId, courseId) ?? throw new ConflictException($"The student is not enrolled to this course");
        }
        public async Task<EnrollmentResponseDto> EnrollAsync(CreateEnrollmentDto dto)
        {
            if (await students.GetByIdAsync(dto.StudentId!.Value) is null)
                throw new NotFoundException("Student not found.");

            var course = await courses.GetByIdAsync(dto.CourseId!.Value);
            if (course is null)
                throw new NotFoundException("Course not found.");

            var dto = new CreateEnrollmentDto(studentId, courseId);

            await createValidator.ValidateAndThrowAsync(dto);

            var enrollment = dto.ToEntity();
            var created = await enrollments.EnrollAsync(enrollment); // unique index: already enrolled → 409

            return created.ToDto();
        }
        public async Task ChangeAsync(int id, UpdateEnrollmentDto input)
        {
            await updateValidator.ValidateAndThrowAsync(input);

            var enrollment = await enrollments.GetByIdAsync(id);

            enrollment.CourseId = input.CourseId!.Value;
            enrollment.StudentId = input.StudentId!.Value;

            await enrollments.ChangeAsync(enrollment);
        }
        public async Task DeleteAsync(int id)
        {
            await cancelValidator.ValidateAndThrowAsync(dto);

            var enrollment = await enrollments.GetByIdAsync(id);

            await enrollments.CancelAsync(enrollment!.Id);
        }
    }
    // Three exceptions, three status codes: 404, 404, and 409 Conflict. The 409 comes from the unique index, caught in the repository
}