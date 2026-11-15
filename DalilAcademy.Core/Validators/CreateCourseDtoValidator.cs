using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator()
        {
            RuleFor(x => x.Price)
                //.Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Price is required")
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative value")
                .LessThanOrEqualTo(50000).WithMessage("Price is too high");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.InstructorId)
                .NotNull().WithMessage("Instructor ID is required")
                .GreaterThan(0).WithMessage("Invalid Instructor ID");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500).WithMessage("ImageUrl is too long")
                .Must(url => url is null || (url.StartsWith("/images/courses/") && !url.Contains("..")))
                .WithMessage("ImageUrl must come from the upload endpoint");
            // without it, a client could send "../../appsettings.json" as an image URL — and a later delete would remove it.
        }
    }
}
