using OCMS.Application.DTO;
using OCMS.Domain.Interfaces;
using OCMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IUnitOfWork _uow;

        public ProgressService(IUnitOfWork uow) => _uow = uow;

        public async Task<CompleteLessonResult> CompleteLessonAsync(Guid lessonId, Guid studentId, CancellationToken ct)
        {
            var lesson = await _uow.Lessons.GetByIdAsync(lessonId, ct)
                         ?? throw new KeyNotFoundException("Lesson not found.");

            var courseId = lesson.CourseId;

            var enrolled = await _uow.Enrollments.ExistsAsync(courseId, studentId, ct);
            if (!enrolled) throw new UnauthorizedAccessException();

            var progress = await _uow.LessonProgress.GetAsync(lessonId, studentId, ct);

            if (progress is not null && progress.CompletedAt != null)
                throw new InvalidOperationException("Lesson already completed.");

            if (progress is null)
            {
                progress = new LessonProgress
                {
                    Id = Guid.NewGuid(),
                    LessonId = lessonId,
                    StudentId = studentId,
                    CompletedAt = DateTime.UtcNow
                };
                await _uow.LessonProgress.AddAsync(progress, ct);
            }
            else
            {
                progress.CompletedAt = DateTime.UtcNow;
                _uow.LessonProgress.Update(progress);
            }

            await _uow.CommitAsync(ct);

            return new CompleteLessonResult(
                lessonId,
                "Completed",
                progress.CompletedAt?.ToUniversalTime()
            );

        }

        public async Task<MyCourseProgressDto> GetMyCourseProgressAsync(Guid courseId, Guid studentId, CancellationToken ct)
        {
            var enrolled = await _uow.Enrollments.ExistsAsync(courseId, studentId, ct);
            if (!enrolled) throw new UnauthorizedAccessException();

            var total = await _uow.LessonProgress.CountLessonsInCourseAsync(courseId, ct);
            if (total == 0)
                return new MyCourseProgressDto(courseId, 0, 0, 0);

            var completed = await _uow.LessonProgress.CountCompletedInCourseAsync(courseId, studentId, ct);

            var percentage = (double)completed / total * 100.0;
            return new MyCourseProgressDto(courseId, total, completed, Math.Round(percentage, 2));
        }

        public async Task<List<TranscriptItemDto>> GetMyTranscriptAsync(Guid studentId, CancellationToken ct)
        {
            var enrollments = await _uow.Enrollments.ListForStudentAsync(studentId, ct);

            var items = new List<TranscriptItemDto>(enrollments.Count);
            foreach (var e in enrollments)
            {
                var courseId = e.CourseId;

                var total = await _uow.LessonProgress.CountLessonsInCourseAsync(courseId, ct);
                var completed = (total == 0)
                    ? 0
                    : await _uow.LessonProgress.CountCompletedInCourseAsync(courseId, studentId, ct);

                var pct = (total == 0) ? 0.0 : Math.Round((double)completed / total * 100.0, 2);

                items.Add(new TranscriptItemDto(
                    courseId,
                    e.Course.Title,
                    total,
                    completed,
                    pct
                ));
            }

            return items;
        }


    }
}
