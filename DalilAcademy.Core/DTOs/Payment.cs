namespace DalilAcademy.Core.DTOs
{
    public record CreatePayPalOrderResponseDto(string OrderId, string ApprovalLink);
    public record PaymentOrderResponseDto(int PaymentId, string OrderId, decimal Amount, string Currency, string ApprovalLink);
}
