
using DalilAcademy.Core.DTOs;

namespace DalilAcademy.Core.Service
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<StudentResponseDto> RegisterStudentByAdminAsync(CreateStudentDto dto);
        Task<LoginResponseDto> RegisterStudentAsync(CreateStudentDto dto);
        Task<InstructorResponseDto> RegisterInstructorByAdminAsync(CreateInstructorDto dto);
        Task<LoginResponseDto> UpdatePasswordAsync(UpdatePasswordDto dto);
    }
}
