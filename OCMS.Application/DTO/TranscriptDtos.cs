using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.DTO
{
    public record TranscriptItemDto(Guid CourseId, string CourseTitle, int TotalLessons, int CompletedLessons, double ProgressPercentage);
}
