using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Security;
using DalilAcademy.Core.Service;
using FluentValidation;

namespace DalilAcademy.Infra.Service
{
    public class AuthService(IUserRepository users, IPasswordHasher hasher, ITokenService tokens, IStudentRepository studentRepository, IValidator<RegisterStudentDto> validator) : IAuthService
    {
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await users.GetByEmailAsync(dto.Email);

            if (user is null || !hasher.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            var (token, expiresAt) = tokens.CreateToken(user);
            return new LoginResponseDto(token, expiresAt);
        }

        public async Task<LoginResponseDto> RegisterStudentAsync(RegisterStudentDto dto)
        {
            await validator.ValidateAndThrowAsync(dto);

            var student = dto.ToStudentEntity();
            var user = dto.ToUserEntity();
            user.PasswordHash = hasher.Hash(dto.Password);

            student.User = user;
            await studentRepository.CreateAsync(student);

            var (token, expiresAt) = tokens.CreateToken(user);
            return new LoginResponseDto(token, expiresAt);
        }
    }
}
