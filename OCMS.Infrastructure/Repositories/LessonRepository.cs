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
    public class LessonRepository : BaseRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(AppDbContext db) : base(db) { }

        public Task<List<Lesson>> ListByCourseAsync(Guid courseId, CancellationToken ct = default)
            => _set.Where(l => l.CourseId == courseId)
                   .OrderBy(l => l.Title)
                   .ToListAsync(ct);

        public Task<Lesson?> GetForCourseAsync(Guid courseId, Guid lessonId, CancellationToken ct = default)
            => _set.FirstOrDefaultAsync(l => l.Id == lessonId && l.CourseId == courseId, ct);
    }
}

