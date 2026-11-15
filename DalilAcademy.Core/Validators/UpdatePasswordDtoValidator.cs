using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class UpdatePasswordDtoValidator : AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePasswordDtoValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
                .EmailAddress().WithMessage("Email address must be valid");

            RuleFor(p => p.OldPassword)
                .NotEmpty().WithMessage("Old Password is required")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters");

            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("New Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters")
                .Must(PasswordValidator.BeAValidPassword).WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number")
                .NotEqual(p => p.OldPassword).WithMessage("Password cannot be the same as the old password");

            RuleFor(p => p.ConfirmNewPassword)
                .NotEmpty().WithMessage("Confirm Password is required")
                .Equal(p => p.NewPassword).WithMessage("The confirmed password does not match the new password");
        }
    }
}
