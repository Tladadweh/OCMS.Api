using OCMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.Services
{
    public interface ICourseQueryService
    {
        Task<List<CourseListItemDto>> SearchAsync(string? keyword, string? category, Guid? instructorId, string? actorRole, CancellationToken ct);
        Task<List<CourseTopItemDto>> TopAsync(int take, CancellationToken ct);

    }
}
