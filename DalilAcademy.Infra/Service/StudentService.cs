using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using FluentValidation;

namespace DalilAcademy.Infra.Service
{
    public class StudentService(IStudentRepository studentRepository, IValidator<CreateStudentDto> validator) : IStudentService
    {
        public Task<List<StudentResponseDto>> GetAllAsync()
        {
            return studentRepository.GetAllAsync();
        }
        public Task<PageResult<StudentResponseDto>> GetPageAsync(int page, int pageSize)
        {
            return studentRepository.GetPageAsync(page, pageSize);
        }
        public Task<PageResult<CourseResponseDto>> GetCoursesInPageAsync(int id, int page, int pageSize)
        {
            return studentRepository.GetCoursesInPageAsync(id, page, pageSize);
        }
        public async Task<StudentResponseDto?> GetByIdAsync(int id)
        {
            var student = await studentRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Student with ID {id} was not found");
            return student?.ToDto();
        }
        public async Task<StudentResponseDto> CreateAsync(CreateStudentDto dto, string hashPass)
        {
            var created = await CreateStudentUserCoreAsync(dto, hashPass, requireUpdatingPassword: false);
            return created.ToDto();
        }
        public async Task<User> RegisterAsync(CreateStudentDto dto, string hashPass)
        {
            var created = await CreateStudentUserCoreAsync(dto, hashPass, requireUpdatingPassword: true);
            return created.ToUserEntity();
        }
        private async Task<Student> CreateStudentUserCoreAsync(CreateStudentDto dto, string hashPass, bool requireUpdatingPassword)
        {
            await validator.ValidateAndThrowAsync(dto);

            var student = dto.ToEntity();
            student.User = dto.ToUserEntity(hashPass);
            student.User.RequireUpdatingPassword = requireUpdatingPassword;
            var created = await studentRepository.CreateAsync(student);

            return created;
        }
        public async Task UpdateAsync(int id, UpdateStudentDto input)
        {
            var student = await studentRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Student with ID {id} was not found"); ;

            student.FullName = input.FullName;
            student.Email = input.Email;

            await studentRepository.UpdateAsync(student);
        }
        public async Task DeleteAsync(int id)
        {
            if (!await studentRepository.DeleteAsync(id))
                throw new NotFoundException($"Student with ID {id} was not found");
        }
    }
}
