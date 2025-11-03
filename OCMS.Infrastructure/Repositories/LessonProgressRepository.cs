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
    public class LessonProgressRepository : BaseRepository<LessonProgress>, ILessonProgressRepository
    {
        public LessonProgressRepository(AppDbContext db) : base(db) { }

        public Task<LessonProgress?> GetAsync(Guid lessonId, Guid studentId, CancellationToken ct = default)
            => _set.FirstOrDefaultAsync(p => p.LessonId == lessonId && p.StudentId == studentId, ct);

        public async Task<int> CountCompletedInCourseAsync(Guid courseId, Guid studentId, CancellationToken ct = default)
        {
            return await _set
                .Where(p => p.StudentId == studentId && p.Lesson.CourseId == courseId && p.CompletedAt != null)
                .CountAsync(ct);
        }

        public Task<int> CountLessonsInCourseAsync(Guid courseId, CancellationToken ct = default)
            => _db.Lessons.Where(l => l.CourseId == courseId).CountAsync(ct);
    }
}
