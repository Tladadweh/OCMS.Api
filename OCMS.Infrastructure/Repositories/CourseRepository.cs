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
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext db) : base(db) { }

        public Task<bool> CodeExistsAsync(string code, CancellationToken ct = default)
            => _set.AnyAsync(c => c.Code == code, ct);

        public Task<Course?> GetByIdWithTrackingAsync(Guid id, CancellationToken ct = default)
            => _set.FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<List<Course>> SearchAsync( string? keyword, string? category, Guid? instructorId, bool onlyPublished, CancellationToken ct = default)
        {
            IQueryable<Course> q = _set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var k = keyword.Trim();
                q = q.Where(c => c.Title.Contains(k) || c.Description!.Contains(k));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                var cat = category.Trim();
                q = q.Where(c => c.Category == cat);
            }

            if (instructorId.HasValue && instructorId.Value != Guid.Empty)
            {
                q = q.Where(c => c.InstructorId == instructorId.Value);
            }

            if (onlyPublished)
            {
                q = q.Where(c => c.IsPublished);
            }

            q = q.OrderByDescending(c => c.IsPublished).ThenBy(c => c.Title);

            return await q.ToListAsync(ct);
        }

        public async Task<List<(Course Course, int EnrollmentCount)>> TopEnrolledAsync(int take, CancellationToken ct = default)
        {
            var data = await _set
                .Where(c => c.IsPublished)  
                .Select(c => new { Course = c, Count = c.Enrollments.Count() })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Course.Title)
                .Take(take > 0 ? take : 10)
                .ToListAsync(ct);

            return data.Select(x => (x.Course, x.Count)).ToList();
        }


    }
}
