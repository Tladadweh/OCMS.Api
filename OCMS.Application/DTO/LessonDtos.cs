using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.DTO
{
    public record LessonCreateDto(string Title, string? Content, int Duration);
    public record LessonUpdateDto(string Title, string? Content, int Duration);
    public record LessonDto(Guid Id, Guid CourseId, string Title, string? Content, int Duration);

}
