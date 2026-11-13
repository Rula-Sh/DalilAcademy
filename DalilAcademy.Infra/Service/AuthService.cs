using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Security;
using DalilAcademy.Core.Service;
using FluentValidation;

namespace DalilAcademy.Infra.Service
{
    public class AuthService(IUserRepository users, IPasswordHasher hasher, ITokenService tokens, IStudentService studentService, IValidator<UpdatePasswordDto> updatePassValidator, IValidator<LoginRequestDto> loginValidator) : IAuthService
    {
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            await loginValidator.ValidateAndThrowAsync(dto);

            var user = await users.GetByEmailAsync(dto.Email);

            if (user is null || !hasher.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid email or password.");

            if(!user.RequireUpdatingPassword)
                throw new ForbiddenException("You must update your password before logging in.");

            var (token, expiresAt) = tokens.CreateToken(user);
            return new LoginResponseDto(token, expiresAt);
        }
        public async Task<StudentResponseDto> RegisterStudentByAdminAsync(CreateStudentDto dto)
        {
            var hash = hasher.Hash(dto.Password);
            var created = await studentService.CreateAsync(dto, hash);
            return created;
        }
        public async Task<LoginResponseDto> RegisterStudentAsync(CreateStudentDto dto)
        {
            var hashPass = hasher.Hash(dto.Password);
            var user = await studentService.RegisterAsync(dto, hashPass);

            var (token, expiresAt) = tokens.CreateToken(user);
            return new LoginResponseDto(token, expiresAt);
        }
        public async Task<LoginResponseDto> UpdatePasswordAsync(UpdatePasswordDto dto)
        {
            await updatePassValidator.ValidateAndThrowAsync(dto);

            var user = await users.GetByEmailAsync(dto.Email);

            if (user is null || !hasher.Verify(dto.OldPassword, user.PasswordHash))
                throw new UnauthorizedException("Invalid email or password.");

            user.PasswordHash = hasher.Hash(dto.NewPassword);
            user.RequireUpdatingPassword = true;

            await users.UpdatePasswordAsync(user);

            var (token, expiresAt) = tokens.CreateToken(user);
            return new LoginResponseDto(token, expiresAt);
        }
    }
}
