using OCMS.Domain.Interfaces;
using OCMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public ICourseRepository Courses { get; }
        public ILessonRepository Lessons { get; }
        public IEnrollmentRepository Enrollments { get; }
        public ILessonProgressRepository LessonProgress { get; }
        public UnitOfWork(AppDbContext db, ICourseRepository courses, ILessonRepository lessons, IEnrollmentRepository enrollments, ILessonProgressRepository lessonProgress)
        {
            _db = db;
            Courses = courses;
            Lessons = lessons;
            Enrollments = enrollments;
            LessonProgress = lessonProgress;
        }

        public Task<int> CommitAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

        public void Dispose() => _db.Dispose();

    }
}
