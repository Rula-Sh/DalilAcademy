namespace DalilAcademy.Core.DTOs
{
    public record CreatePayPalOrderResponseDto(string OrderId, string ApprovalLink);
    public record PaymentOrderResponseDto(int PaymentId, string OrderId, decimal Amount, string Currency, string ApprovalLink);
    public record PaymentResponseDto(int PaymentId, string OrderId, decimal Amount, string Currency, string Status, string CreatedAt, string PaidAt);
}
