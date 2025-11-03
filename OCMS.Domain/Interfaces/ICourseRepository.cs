using OCMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Domain.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<bool> CodeExistsAsync(string code, CancellationToken ct = default);
        Task<Course?> GetByIdWithTrackingAsync(Guid id, CancellationToken ct = default);

        Task<List<Course>> SearchAsync(string? keyword, string? category, Guid? instructorId, bool onlyPublished, CancellationToken ct = default);
        Task<List<(Course Course, int EnrollmentCount)>> TopEnrolledAsync(int take, CancellationToken ct = default);


    }
}
