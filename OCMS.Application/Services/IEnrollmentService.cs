using OCMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.Services
{
    public interface IEnrollmentService
    {
        Task EnrollAsync(Guid courseId, Guid studentId, CancellationToken ct);
        Task<List<MyEnrollmentDto>> ListMineAsync(Guid studentId, CancellationToken ct);

    }
}
