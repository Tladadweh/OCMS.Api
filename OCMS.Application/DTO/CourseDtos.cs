using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.DTO
{
     public class CourseDtos
    {
        public record CourseCreateDto(string Code, string Title, string? Description, string? Category);
        public record CourseUpdateDto(string Title, string? Description, string? Category);
        public record CourseDetailsDto(Guid Id, string Code, string Title, string? Description, string? Category, bool IsPublished, Guid InstructorId);

    }
}
