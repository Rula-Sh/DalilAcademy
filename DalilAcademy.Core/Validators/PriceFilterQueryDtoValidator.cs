using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class PriceFilterQueryDtoValidator : AbstractValidator<PriceFilterQueryDto>
    {
        public PriceFilterQueryDtoValidator()
        {
            RuleFor(pf => pf.Min)
                .GreaterThanOrEqualTo(0).WithMessage("Min price must be a non-negative value");

            RuleFor(pf => pf.Max)
                .GreaterThanOrEqualTo(1).WithMessage("Max price must be a non-negative value")
                .GreaterThanOrEqualTo(pf => pf.Min).WithMessage("Cannot set the max price value less than the min");
        }
    }
}
