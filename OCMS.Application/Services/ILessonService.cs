using OCMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.Services
{
    public interface ILessonService
    {
        Task<LessonDto> CreateAsync(Guid courseId, LessonCreateDto dto, Guid actorId, string actorRole, CancellationToken ct);
        Task<List<LessonDto>> ListAsync(Guid courseId, CancellationToken ct);
        Task<LessonDto?> UpdateAsync(Guid courseId, Guid lessonId, LessonUpdateDto dto, Guid actorId, string actorRole, CancellationToken ct);
        Task<bool> DeleteAsync(Guid courseId, Guid lessonId, Guid actorId, string actorRole, CancellationToken ct);
    }
}
