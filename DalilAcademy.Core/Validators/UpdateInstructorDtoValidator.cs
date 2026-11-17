using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class UpdateInstructorDtoValidator : AbstractValidator<UpdateInstructorDto>
    {
        public UpdateInstructorDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
                .EmailAddress().WithMessage("Email address must be valid");
        }
    }
}