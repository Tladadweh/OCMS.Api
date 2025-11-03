using OCMS.Application.DTO;
using OCMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace OCMS.Application.Services
{
    public class RosterService : IRosterService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _map;


        public RosterService(IUnitOfWork uow, IMapper map)
        {
            _uow = uow;
            _map = map;
        }

        public async Task<List<RosterStudentDto>> GetRosterAsync(Guid courseId, Guid actorId, string actorRole, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(courseId, ct)
                         ?? throw new KeyNotFoundException("Course not found.");

            var isOwner = course.InstructorId == actorId;
            var isAdmin = string.Equals(actorRole, "Admin", StringComparison.Ordinal);
            if (!isOwner && !isAdmin)
                throw new UnauthorizedAccessException();

            var rows = await _uow.Enrollments.ListForCourseAsync(courseId, ct);

            return _map.Map<List<RosterStudentDto>>(rows);

        }

    }
}
