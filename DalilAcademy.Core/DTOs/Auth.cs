namespace DalilAcademy.Core.DTOs
{
    public record LoginRequestDto(string Email, string Password);
    public record LoginResponseDto(string Token, DateTime ExpiresAt);
    public record RegisterStudentDto(string FullName, string Email, string Password);
    public record UserResponseDto(string Email);
}
