using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Mappings
{
    public static class PaymentMappings
    {
        public static PaymentOrderResponseDto ToDto(this Payment payment, CreatePayPalOrderResponseDto orderResponse)
        {
            return new PaymentOrderResponseDto(payment.Id, payment.PayPalOrderId, payment.Amount, payment.Currency, orderResponse.ApprovalLink);
        }
    }
}
