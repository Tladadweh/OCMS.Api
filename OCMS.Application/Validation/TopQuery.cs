using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.Validation
{
    public class TopQuery { public int Take { get; set; } }

    public class TopQueryValidator : AbstractValidator<TopQuery>
    {
        public TopQueryValidator()
        {
            RuleFor(x => x.Take).GreaterThan(0).WithMessage("take must be a positive integer (> 0).");
        }
    }

}
