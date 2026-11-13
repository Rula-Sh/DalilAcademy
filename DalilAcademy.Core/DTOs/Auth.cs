namespace DalilAcademy.Core.DTOs
{
    public record LoginRequestDto(string Email, string Password);
    public record LoginResponseDto(string Token, DateTime ExpiresAt);    
    public record UserResponseDto(string Email);
    public record UpdatePasswordDto(string Email, string OldPassword, string NewPassword, string ConfirmNewPassword);
}
