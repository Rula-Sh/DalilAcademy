using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class UpdateEnrollmentDtoValidator : AbstractValidator<UpdateEnrollmentDto>
    {
        public UpdateEnrollmentDtoValidator()
        {
            RuleFor(x => x.StudentId)
                .NotNull().WithMessage("Student ID is required")
                .GreaterThan(0).WithMessage("Invalid Student ID");

            RuleFor(x => x.CourseId)
                .NotNull().WithMessage("Student ID is required")
                .GreaterThan(0).WithMessage("Invalid Course ID");
        }
    }
}
