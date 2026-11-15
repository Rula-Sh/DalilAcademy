using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class ConvertPriceRequestValidator : AbstractValidator<ConvertPriceRequestDto>
    {
        public ConvertPriceRequestValidator()
        {
            RuleFor(c => c.FromCurrency)
                .NotEmpty().WithMessage("From currency is required")
                .Matches("^[A-Za-z]{3}$").WithMessage("From currency must be a valid 3-letter code (e.g., USD, EUR).");

            RuleFor(c => c.ToCurrency)
                .NotEmpty().WithMessage("To currency is required")
                .NotEqual(c => c.FromCurrency).WithMessage("Source and target currencies must differ")
                .Matches("^[A-Za-z]{3}$").WithMessage("From currency must be a valid 3-letter code (e.g., USD, EUR).");

            RuleFor(c => c.Amount)
                .NotNull().WithMessage("Amount is required")
                .GreaterThan(0).WithMessage("Amount must be greater than 0");
        }
    }
}
