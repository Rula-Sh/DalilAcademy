using DalilAcademy.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalilAcademy.Core.Validators
{
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            RuleFor(x => x.FullName)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(x => x.Email)
                .MaximumLength(256)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
