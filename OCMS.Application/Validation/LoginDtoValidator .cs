using FluentValidation;
using OCMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.Validation
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserNameOrEmail).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }

    }
}
