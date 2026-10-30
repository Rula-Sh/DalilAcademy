using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class CreateEnrollmentDtoValidator : AbstractValidator<CreateEnrollmentDto>
    {
        public CreateEnrollmentDtoValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThanOrEqualTo(1)
                .NotEmpty()
                .WithMessage("Student Id is invalid.");

            RuleFor(x => x.CourseId)
                .GreaterThanOrEqualTo(1)
                .NotEmpty()
                .WithMessage("Student Id is invalid.");
        }
    }
}
