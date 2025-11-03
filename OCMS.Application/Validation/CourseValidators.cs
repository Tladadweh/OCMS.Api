using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OCMS.Application.DTO.CourseDtos;

namespace OCMS.Application.Validation
{
    public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
    {
        public CourseCreateDtoValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.Category).MaximumLength(60);
        }
    }

    public class CourseUpdateDtoValidator : AbstractValidator<CourseUpdateDto>
    {
        public CourseUpdateDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.Category).MaximumLength(60);
        }
    }

}
