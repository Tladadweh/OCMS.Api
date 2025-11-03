using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.DTO
{
        public record CompleteLessonResult(Guid LessonId, string Status, DateTime? CompletedAtUtc);

        public record MyCourseProgressDto(Guid CourseId, int TotalLessons, int CompletedLessons, double Percentage);

    
}
