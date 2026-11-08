using DalilAcademy.Core.DTOs;
using FluentValidation;

namespace DalilAcademy.Core.Validators
{
    public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be zero or above.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.InstructorId)
                .GreaterThan(0);

            RuleFor(x => x.ImageUrl)
                .Must(url => url is null || (url.StartsWith("/images/courses/") && !url.Contains("..")))
                .WithMessage("ImageUrl must come from the upload endpoint."); 
            // without it, a client could send "../../appsettings.json" as an image URL — and a later delete would remove it.
        }
    }
}
