namespace DalilAcademy.Core.DTOs
{
    public record ConvertPriceRequestDto(decimal Amount, string FromCurrency, string ToCurrency);
    public record ConvertPriceResponseDto(decimal OriginalAmount, string FromCurrency, decimal ConvertedAmount, string ToCurrency, decimal ExchangeRate);
}