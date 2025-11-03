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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _uow;

        public EnrollmentService(IUnitOfWork uow) => _uow = uow;

        public async Task EnrollAsync(Guid courseId, Guid studentId, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(courseId, ct)
                         ?? throw new KeyNotFoundException("Course not found.");

            if (!course.IsPublished)
                throw new InvalidOperationException("Course is not published.");

            if (await _uow.Enrollments.ExistsAsync(courseId, studentId, ct))
                throw new InvalidOperationException("Already enrolled.");

            var e = new Enrollment
            {
                Id = Guid.NewGuid(),
                CourseId = courseId,
                StudentId = studentId
            };

            await _uow.Enrollments.AddAsync(e, ct);
            await _uow.CommitAsync(ct);
        }

        public async Task<List<MyEnrollmentDto>> ListMineAsync(Guid studentId, CancellationToken ct)
        {
            var list = await _uow.Enrollments.ListForStudentAsync(studentId, ct);
            return list.Select(e =>
                new MyEnrollmentDto(e.CourseId, e.Course.Title, e.CreatedAt.ToUniversalTime())).ToList();

        }

    }
}
