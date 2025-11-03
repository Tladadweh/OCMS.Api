using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.DTO
{
    public record UpdateMyProfileDto(string? FullName, string? Email, string? Bio, string? PhotoUrl);

}
