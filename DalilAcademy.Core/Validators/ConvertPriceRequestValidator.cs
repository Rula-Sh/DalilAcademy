using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class ConvertPriceRequestValidator : AbstractValidator<ConvertPriceRequestDto>
    {
        public ConvertPriceRequestValidator()
        {
            RuleFor(c => c.FromCurrency).Matches("^[A-Z]{3}$");
            RuleFor(c => c.ToCurrency).Matches("^[A-Z]{3}$");
            RuleFor(c => c.Amount).GreaterThan(0);
        }
    }
}
