using Microsoft.EntityFrameworkCore;
using OCMS.Domain.Interfaces;
using OCMS.Domain.Models;
using OCMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Infrastructure.Repositories
{
    public class EnrollmentRepository : BaseRepository<Enrollment> , IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext db) : base(db) { }

        public Task<bool> ExistsAsync(Guid courseId, Guid studentId, CancellationToken ct = default)
            => _set.AnyAsync(e => e.CourseId == courseId && e.StudentId == studentId, ct);

        public Task<List<Enrollment>> ListForStudentAsync(Guid studentId, CancellationToken ct = default)
            => _set
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync(ct);

        public Task<List<Enrollment>> ListForCourseAsync(Guid courseId, CancellationToken ct = default)
            => _set.Where(e => e.CourseId == courseId)
                   .Include(e => e.Student)
                   .OrderBy(e => e.Student.UserName)
                   .ToListAsync(ct);


    }
}
