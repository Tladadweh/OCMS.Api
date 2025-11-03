using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using OCMS.Application.DTO;

namespace OCMS.Application.Validation
{
    public class UpdateUserRoleDtoValidator : AbstractValidator<UpdateUserRoleDto>
    {
        private static readonly HashSet<string> Allowed =
    new(StringComparer.OrdinalIgnoreCase) { "Admin", "Instructor", "Student" };

        public UpdateUserRoleDtoValidator()
        {
            RuleFor(x => x.Role).NotEmpty().Must(r => Allowed.Contains(r))
                .WithMessage("Role must be one of: Admin, Instructor, Student");
        }

    }
}
