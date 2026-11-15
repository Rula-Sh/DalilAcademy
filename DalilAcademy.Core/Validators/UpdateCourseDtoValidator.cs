using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class UpdateCourseDtoValidator : AbstractValidator<UpdateCourseDto>
    {
        public UpdateCourseDtoValidator()
        {
            RuleFor(x => x.Price)
                .NotNull().WithMessage("Price is required")
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative value")
                .LessThanOrEqualTo(50000).WithMessage("Price is too high");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500).WithMessage("ImageUrl is too long")
                .Must(url => url is null || (url.StartsWith("/images/courses/") && !url.Contains("..")))
                .WithMessage("ImageUrl must come from the upload endpoint");
            // without it, a client could send "../../appsettings.json" as an image URL — and a later delete would remove it.
        }
    }
}
