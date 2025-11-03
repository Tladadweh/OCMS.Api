using OCMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Domain.Interfaces
{
    public interface ILessonProgressRepository : IRepository<LessonProgress>
    {
        Task<LessonProgress?> GetAsync(Guid lessonId, Guid studentId, CancellationToken ct = default);
        Task<int> CountCompletedInCourseAsync(Guid courseId, Guid studentId, CancellationToken ct = default);
        Task<int> CountLessonsInCourseAsync(Guid courseId, CancellationToken ct = default);

    }
}
