using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name field is required")
                .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email field is required")
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
                .EmailAddress().WithMessage("Email address must be valid");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password field is required")
                .MinimumLength(8).WithMessage("Password must contain at least 8 characters")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters")
                .Must(PasswordValidator.BeAValidPassword).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.");
        }
    }
}