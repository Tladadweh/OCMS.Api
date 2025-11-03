using OCMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Domain.Interfaces
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<bool> ExistsAsync(Guid courseId, Guid studentId, CancellationToken ct = default);
        Task<List<Enrollment>> ListForStudentAsync(Guid studentId, CancellationToken ct = default);
        Task<List<Enrollment>> ListForCourseAsync(Guid courseId, CancellationToken ct = default);


    }
}
