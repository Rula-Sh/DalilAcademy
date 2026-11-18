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
            var student = await students.GetByUserIdAsync(userId) ?? throw new NotFoundException($"Student with UserId {userId} was not found.");

            return await enrollments.GetStudentEnrollmentsAsync(student!.Id, page, pageSize);
        }
        public async Task<EnrollmentResponseDto?> GetByIdAsync(int id)
        {
            if (id < 1)
                throw new BadRequestException("Invalid Enrollement ID");

            var enrollment = await enrollments.GetByIdAsync(id) ?? throw new NotFoundException($"Enrollment with ID {id} was not found");
            return enrollment?.ToDto();
        }
        public async Task<List<PaymentResponseDto>> GetPaymentsAsync(int id)
        {
            if (id < 1)
                throw new BadRequestException("Invalid Enrollement ID");

            return await enrollments.GetPaymentsAsync(id);
        }
        public async Task<EnrollmentResponseDto> EnrollAsync(CreateEnrollmentDto dto)
        {

            if (await students.GetByIdAsync(dto.StudentId!.Value) is null)
                throw new NotFoundException("Student not found.");

            if (await courses.GetByIdAsync(dto.CourseId!.Value) is null)
                throw new NotFoundException("Course not found.");

            var course = await courses.GetByIdAsync(dto.CourseId!.Value);

            await createValidator.ValidateAndThrowAsync(dto);

            if (course!.Capacity <= await enrollments.GetEnrolledCountByCourseIdAsync(course.Id))
                throw new ConflictException("This course reached its maximum capacity.");

            var enrollment = dto.ToEntity();
            if (course!.Price == 0)
                enrollment.Status = EnrollmentStatus.Paid;

            var created = await enrollments.EnrollAsync(enrollment); // unique index: already enrolled → 409

            return created.ToDto();
        }
        public async Task ChangeAsync(int id, UpdateEnrollmentDto input)
        {
            await updateValidator.ValidateAndThrowAsync(input);

            var enrollment = await enrollments.GetByIdAsync(id);
            var course = await courses.GetByIdAsync((int)input.CourseId!);

            if (enrollment!.Status == EnrollmentStatus.Paid)
                throw new ConflictException($"Cannot changed enrollment after payment");

            if (course!.Capacity <= await enrollments.GetEnrolledCountByCourseIdAsync(course.Id))
                throw new ConflictException("This course reached its max capacity.");

            if (course!.Price == 0) enrollment!.Status = EnrollmentStatus.Paid;
            enrollment.CourseId = input.CourseId!.Value;
            enrollment.StudentId = input.StudentId!.Value;

            await enrollments.ChangeAsync(enrollment);
        }
        public async Task DeleteAsync(int id)
        {
            var enrollment = await enrollments.GetByIdAsync(id);

            if (enrollment!.Status == EnrollmentStatus.Paid)
                throw new ConflictException($"Cannot cancel enrollment after payment");

            await enrollments.CancelAsync(enrollment!.Id);
        }
    }
    // Three exceptions, three status codes: 404, 404, and 409 Conflict. The 409 comes from the unique index, caught in the repository
}
