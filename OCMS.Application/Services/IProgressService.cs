using OCMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OCMS.Application.Services
{
    public interface IProgressService
    {
        Task<CompleteLessonResult> CompleteLessonAsync(Guid lessonId, Guid studentId, CancellationToken ct);
        Task<MyCourseProgressDto> GetMyCourseProgressAsync(Guid courseId, Guid studentId, CancellationToken ct);
        Task<List<TranscriptItemDto>> GetMyTranscriptAsync(Guid studentId, CancellationToken ct);

    }
}
