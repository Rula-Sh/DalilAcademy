using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Security;
using DalilAcademy.Core.Service;

namespace DalilAcademy.Infra.Service
{
    public class AuthService(IUserRepository users, IPasswordHasher hasher, ITokenService tokens) : IAuthService
    {
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await users.GetByEmailAsync(dto.Email);
            
            if (user is null || !hasher.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");
            
            var (token, expiresAt) = tokens.CreateToken(user);

            return new LoginResponseDto(token, expiresAt);

        }
    }
}
