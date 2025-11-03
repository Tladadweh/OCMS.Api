using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using OCMS.Application.DTO;


namespace OCMS.Application.Validation
{
    public class UpdateMyProfileDtoValidator : AbstractValidator<UpdateMyProfileDto>
    {
        public UpdateMyProfileDtoValidator()
        {
            RuleFor(x => x.FullName).MaximumLength(80);
            When(x => x.Email is not null, () =>
            {
                RuleFor(x => x.Email!).NotEmpty().EmailAddress().MaximumLength(120);
            });
            RuleFor(x => x.Bio).MaximumLength(500);
            When(x => x.PhotoUrl is not null, () =>
            {
                RuleFor(x => x.PhotoUrl!).MaximumLength(300).Must(IsHttpUrl)
                    .WithMessage("PhotoUrl must be a valid http/https URL.");
            });
        }

        private static bool IsHttpUrl(string url)
            => Uri.TryCreate(url, UriKind.Absolute, out var u)
               && (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps);
    }

}

