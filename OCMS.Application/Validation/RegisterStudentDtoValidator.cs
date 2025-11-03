using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using OCMS.Application.DTO;

namespace OCMS.Application.Validation
{
    public class RegisterStudentDtoValidator : AbstractValidator<RegisterStudentDto>
    {
        public RegisterStudentDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(3).MaximumLength(40);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(120);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
        }
    }
}
