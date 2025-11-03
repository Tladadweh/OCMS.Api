using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OCMS.Application.DTO.CourseDtos;

namespace OCMS.Application.Services
{
    public interface ICourseService
    {
        Task<CourseDetailsDto> CreateAsync(CourseCreateDto dto, Guid instructorId, CancellationToken ct);
        Task<CourseDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<CourseDetailsDto?> UpdateAsync(Guid id, CourseUpdateDto dto, Guid actorId, string actorRole, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, Guid actorId, string actorRole, CancellationToken ct);
        Task<CourseDetailsDto> PublishAsync(Guid id, Guid actorId, string actorRole, CancellationToken ct);


    }
}
