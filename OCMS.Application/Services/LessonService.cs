using AutoMapper;
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
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _map;

        public LessonService(IUnitOfWork uow, IMapper map)
        {
            _uow = uow;
            _map = map;
        }

        public async Task<LessonDto> CreateAsync(Guid courseId, LessonCreateDto dto, Guid actorId, string actorRole, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(courseId, ct)
                         ?? throw new KeyNotFoundException("Course not found.");

            var isOwner = course.InstructorId == actorId;
            var isAdmin = string.Equals(actorRole, "Admin", StringComparison.Ordinal);
            if (!isOwner && !isAdmin) throw new UnauthorizedAccessException();

            var lesson = _map.Map<Lesson>(dto);
            lesson.Id = Guid.NewGuid();
            lesson.CourseId = courseId;

            await _uow.Lessons.AddAsync(lesson, ct);
            await _uow.CommitAsync(ct);

            return _map.Map<LessonDto>(lesson);
        }

        public async Task<List<LessonDto>> ListAsync(Guid courseId, CancellationToken ct)
        {
            var list = await _uow.Lessons.ListByCourseAsync(courseId, ct);
            return _map.Map<List<LessonDto>>(list);
        }

        public async Task<LessonDto?> UpdateAsync(Guid courseId, Guid lessonId, LessonUpdateDto dto, Guid actorId, string actorRole, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(courseId, ct);
            if (course is null) return null;

            var isOwner = course.InstructorId == actorId;
            var isAdmin = string.Equals(actorRole, "Admin", StringComparison.Ordinal);
            if (!isOwner && !isAdmin) throw new UnauthorizedAccessException();

            var lesson = await _uow.Lessons.GetForCourseAsync(courseId, lessonId, ct);
            if (lesson is null) return null;

            _map.Map(dto, lesson);
            _uow.Lessons.Update(lesson);
            await _uow.CommitAsync(ct);

            return _map.Map<LessonDto>(lesson);
        }

        public async Task<bool> DeleteAsync(Guid courseId, Guid lessonId, Guid actorId, string actorRole, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(courseId, ct);
            if (course is null) return false;

            var isOwner = course.InstructorId == actorId;
            var isAdmin = string.Equals(actorRole, "Admin", StringComparison.Ordinal);
            if (!isOwner && !isAdmin) throw new UnauthorizedAccessException();

            var lesson = await _uow.Lessons.GetForCourseAsync(courseId, lessonId, ct);
            if (lesson is null) return false;

            _uow.Lessons.Remove(lesson);
            await _uow.CommitAsync(ct);
            return true;
        }
    }
}
