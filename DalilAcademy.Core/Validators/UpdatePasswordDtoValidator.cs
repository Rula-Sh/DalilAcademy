using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class UpdatePasswordDtoValidator : AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePasswordDtoValidator()
        {
            RuleFor(p => p.Email)
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
                .NotEmpty().WithMessage("Email field is required")
                .EmailAddress().WithMessage("Email address must be valid");

            RuleFor(p => p.OldPassword)
                .NotEmpty().WithMessage("Old Password field is required");

            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("Password field is required")
                .MinimumLength(8).WithMessage("New Password must contain at least 8 characters")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters")
                .Must(PasswordValidator.BeAValidPassword).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number.")
                .NotEqual(p => p.OldPassword).WithMessage("Password cannot be the same as the old password");

            RuleFor(p => p.ConfirmNewPassword)
                .NotEmpty().WithMessage("Confirm Password field is required")
                .Equal(p => p.NewPassword).WithMessage("The confirmed password does not match the new password");
        }
    }
}
