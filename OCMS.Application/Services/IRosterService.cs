using OCMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.Services
{
    public interface IRosterService
    {
        Task<List<RosterStudentDto>> GetRosterAsync(Guid courseId, Guid actorId, string actorRole, CancellationToken ct);

    }
}
