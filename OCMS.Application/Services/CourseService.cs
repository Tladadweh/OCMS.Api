using AutoMapper;
using OCMS.Domain.Interfaces;
using OCMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OCMS.Application.DTO.CourseDtos;

namespace OCMS.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _map;

        public CourseService(IUnitOfWork uow, IMapper map)
        {
            _uow = uow;
            _map = map;
        }

        public async Task<CourseDetailsDto> CreateAsync(CourseCreateDto dto, Guid instructorId, CancellationToken ct)
        {
            if (await _uow.Courses.CodeExistsAsync(dto.Code.Trim(), ct))
                throw new InvalidOperationException("Course code already exists.");

            var course = _map.Map<Course>(dto);
            course.Id = Guid.NewGuid();
            course.InstructorId = instructorId;

            await _uow.Courses.AddAsync(course, ct);
            await _uow.CommitAsync(ct);

            return _map.Map<CourseDetailsDto>(course);
        }

        public async Task<CourseDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var c = await _uow.Courses.GetByIdAsync(id, ct); 
            return c is null ? null : _map.Map<CourseDetailsDto>(c); 
        }


        public async Task<CourseDetailsDto?> UpdateAsync(Guid id, CourseUpdateDto dto, Guid actorId, string actorRole, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdWithTrackingAsync(id, ct);
            if (course is null) return null;

            var isOwner = course.InstructorId == actorId;
            var isAdmin = string.Equals(actorRole, "Admin", StringComparison.Ordinal);
            if (!isOwner && !isAdmin) throw new UnauthorizedAccessException();


            if (course.IsPublished)
                throw new InvalidOperationException("Course is already published; updates are not allowed.");

            _map.Map(dto, course);
            _uow.Courses.Update(course);
            await _uow.CommitAsync(ct);

            return _map.Map<CourseDetailsDto>(course);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid actorId, string actorRole, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdWithTrackingAsync(id, ct);
            if (course is null) return false;

            var isOwner = course.InstructorId == actorId;
            var isAdmin = string.Equals(actorRole, "Admin", StringComparison.Ordinal);
            if (!isOwner && !isAdmin) throw new UnauthorizedAccessException();

            _uow.Courses.Remove(course);
            await _uow.CommitAsync(ct);
            return true;
        }

        public async Task<CourseDetailsDto> PublishAsync(Guid id, Guid actorId, string actorRole, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdWithTrackingAsync(id, ct)
                         ?? throw new KeyNotFoundException("Course not found.");

            var isOwner = course.InstructorId == actorId;
            var isInstructor = string.Equals(actorRole, "Instructor", StringComparison.Ordinal);
            if (!isOwner || !isInstructor)
                throw new UnauthorizedAccessException();

            if (course.IsPublished)
                throw new InvalidOperationException("Course already published.");

            var lessons = await _uow.Lessons.ListByCourseAsync(id, ct);
            if (lessons.Count == 0)
                throw new InvalidOperationException("Cannot publish a course without lessons.");

            course.IsPublished = true;

            _uow.Courses.Update(course);
            await _uow.CommitAsync(ct);

            return _map.Map<CourseDetailsDto>(course);
        }

    }
}
