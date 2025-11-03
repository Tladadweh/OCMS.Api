using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.DTO
{
    public record CourseListItemDto( Guid Id, string Title, string? Description, string? Category, bool IsPublished, Guid InstructorId);

}
