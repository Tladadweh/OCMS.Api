using AutoMapper;
using OCMS.Application.DTO;
using OCMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.Services
{
    public class CourseQueryService : ICourseQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _map;


        public CourseQueryService(IUnitOfWork uow, IMapper map)
        {
            _uow = uow;
            _map = map;
        }

        public async Task<List<CourseListItemDto>> SearchAsync(
            string? keyword,
            string? category,
            Guid? instructorId,
            string? actorRole,
            CancellationToken ct)
        {
            var onlyPublished = string.Equals(actorRole, "Student", StringComparison.Ordinal) || actorRole is null;

            var list = await _uow.Courses.SearchAsync(keyword, category, instructorId, onlyPublished, ct);

            return _map.Map<List<CourseListItemDto>>(list);
        }

        public async Task<List<CourseTopItemDto>> TopAsync(int take, CancellationToken ct)
        {

            var rows = await _uow.Courses.TopEnrolledAsync(take, ct);
            return _map.Map<List<CourseTopItemDto>>(rows);
        }


    }
}
