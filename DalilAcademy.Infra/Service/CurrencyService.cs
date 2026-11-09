using DalilAcademy.Core.Service;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using DalilAcademy.Core.DTOs;

namespace DalilAcademy.Infra.Service
{
    public class CurrencyService(HttpClient http) : ICurrencyService // http to get data from exchangerate-api website
    {
        private record PairResponse([property: JsonPropertyName("conversion_rate")] decimal ConversionRate); // gets the conversion_rate from exchangerate-api website
        public async Task<ConvertPriceResponseDto> ConvertAsync(ConvertPriceRequestDto request)
        {
            var data = await http.GetFromJsonAsync<PairResponse>($"pair/{request.FromCurrency}/{request.ToCurrency}") // prepare the URL request https://v6.exchangerate-api.com/v6/YOUR-KEY/pair/JOD/USD then sends a get request
                ?? throw new HttpRequestException("Empty response from ExchangeRate-API");

            var converted = Math.Round(request.Amount * data.ConversionRate, 2); // calculate the (amount * conversion rate).xx
            return new ConvertPriceResponseDto(request.Amount, request.FromCurrency, converted, request.ToCurrency, data.ConversionRate);
        }
    }
}