using DalilAcademy.Core.DTOs;

namespace DalilAcademy.Core.Payments
{
    public interface IPaymentGateway
    {
        Task<CreatePayPalOrderResponseDto> CreateOrderAsync(decimal amountUsd);
        Task<bool> CaptureOrderAsync(string orderId);
    }
}
