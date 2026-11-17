using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using FluentValidation;

namespace DalilAcademy.Infra.Service
{
    public class InstructorService(
            IInstructorRepository InstructorRepository,
            IUserRepository userRepository,
            IValidator<CreateInstructorDto> CreateValidator,
            IValidator<UpdateInstructorDto> UpdateValidator
        ) : IInstructorService
    {
        public Task<List<InstructorResponseDto>> GetAllAsync()
        {
            return InstructorRepository.GetAllAsync();
        }
        public Task<PageResult<InstructorResponseDto>> GetPageAsync(int page, int pageSize)
        {
            return InstructorRepository.GetPageAsync(page, pageSize);
        }
        public async Task<PageResult<CourseResponseDto>> GetCoursesInPageAsync(int id, int page, int pageSize)
        {
            if (id < 1)
                throw new BadRequestException("Invalid Instructor ID");
            await GetByIdAsync(id);

            return await InstructorRepository.GetCoursesInPageAsync(id, page, pageSize);
        }
        public async Task<InstructorResponseDto?> GetByIdAsync(int id)
        {
            if (id < 1)
                throw new BadRequestException("Invalid Instructor ID");

            var instructor = await InstructorRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Instructor with ID {id} was not found");
            return instructor?.ToDto();
        }
        public async Task<InstructorResponseDto> CreateAsync(CreateInstructorDto dto, string hashPass)
        {
            var created = await CreateInstructorUserCoreAsync(dto, hashPass, requireUpdatingPassword: false);
            return created.ToDto();
        }
        public async Task<User> RegisterAsync(CreateInstructorDto dto, string hashPass)
        {
            var created = await CreateInstructorUserCoreAsync(dto, hashPass, requireUpdatingPassword: true);
            return created.ToUserEntity();
        }
        private async Task<Instructor> CreateInstructorUserCoreAsync(CreateInstructorDto dto, string hashPass, bool requireUpdatingPassword)
        {
            await CreateValidator.ValidateAndThrowAsync(dto);

            var instructor = dto.ToEntity(dto.ToUserEntity(hashPass));
            instructor.User.RequireUpdatingPassword = requireUpdatingPassword;
            var created = await InstructorRepository.CreateAsync(instructor);

            return created;
        }
        public async Task UpdateAsync(int id, UpdateInstructorDto input)
        {
            await UpdateValidator.ValidateAndThrowAsync(input);

            var instructor = await InstructorRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Instructor with ID {id} was not found");
            var user = await userRepository.GetByEmailAsync(input.Email);
            if (user is not null && user.Email != input.Email)
                throw new ConflictException($"A user with this email already exists.");

            instructor.FullName = input.FullName;
            instructor.User.Email = input.Email;

            await InstructorRepository.UpdateAsync(instructor);
        }
        public async Task DeleteAsync(int id)
        {
            if (!await InstructorRepository.DeleteAsync(id))
                throw new NotFoundException($"Instructor with ID {id} was not found");
        }
    }
}
