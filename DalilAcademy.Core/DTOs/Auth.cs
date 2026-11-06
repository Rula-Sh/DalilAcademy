namespace DalilAcademy.Core.DTOs
{
    public record LoginRequestDto(string Email, string Password);
    public record LoginResponseDto(string Token, DateTime ExpiresAt);
}
