using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class RegisterStudentDtoValidator : AbstractValidator<RegisterStudentDto>
    {
        public RegisterStudentDtoValidator()
        {
            RuleFor(x => x.FullName)
                .MaximumLength(200)
                .NotEmpty().WithMessage("Full name field is required");

            RuleFor(x => x.Email)
                .MaximumLength(256)
                .NotEmpty().WithMessage("Email field is required")
                .EmailAddress().WithMessage("Email address must be valid");

            RuleFor(x => x.Password)
                 .NotEmpty().WithMessage("Password field is required")
                 .MinimumLength(8).WithMessage("Password must contain at least 8 characters")
                 .Must(BeAValidPassword).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.");
        }

        private bool BeAValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            return hasUpper && hasLower && hasDigit;
        }
    }
}
