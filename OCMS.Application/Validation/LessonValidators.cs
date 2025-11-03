using FluentValidation;
using OCMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.Validation
{
    public class LessonCreateDtoValidator : AbstractValidator<LessonCreateDto>
    {
        public LessonCreateDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Content).MaximumLength(4000);
            RuleFor(x => x.Duration).GreaterThan(0);
        }
    }

    public class LessonUpdateDtoValidator : AbstractValidator<LessonUpdateDto>
    {
        public LessonUpdateDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Content).MaximumLength(4000);
            RuleFor(x => x.Duration).GreaterThan(0);
        }
    }
    }
