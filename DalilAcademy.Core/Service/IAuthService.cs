
using DalilAcademy.Core.DTOs;
namespace DalilAcademy.Core.Service
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<LoginResponseDto> RegisterStudentAsync(RegisterStudentDto dto);
    }
}
