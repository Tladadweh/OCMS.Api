using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Courses { get; }
        ILessonRepository Lessons { get; }
        IEnrollmentRepository Enrollments { get; }
        ILessonProgressRepository LessonProgress { get; }
        Task<int> CommitAsync(CancellationToken ct = default);

    }
}
