using DalilAcademy.Core.DTOs;

namespace DalilAcademy.Core.Service
{
    public interface ICurrencyService
    {
        Task<ConvertPriceResponseDto> ConvertAsync(ConvertPriceRequestDto request);
    }
}