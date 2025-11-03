using OCMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Domain.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<List<Lesson>> ListByCourseAsync(Guid courseId, CancellationToken ct = default);
        Task<Lesson?> GetForCourseAsync(Guid courseId, Guid lessonId, CancellationToken ct = default);

    }
}
