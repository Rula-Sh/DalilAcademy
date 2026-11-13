using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email field is required")
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
                .EmailAddress().WithMessage("Email address must be valid");

            RuleFor(p => p.Password)
                 .NotEmpty().WithMessage("Password field is required")
                 .MaximumLength(100).WithMessage("Password cannot exceed 100 characters");
        }
    }
}
