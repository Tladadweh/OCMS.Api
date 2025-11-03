using AutoMapper;
using OCMS.Application.DTO;
using OCMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Application.Mapping
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            // DTO -> Entity
            CreateMap<LessonCreateDto, Lesson>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CourseId, opt => opt.Ignore())
                .ForMember(d => d.EstimatedMinutes, opt => opt.MapFrom(s => s.Duration));

            CreateMap<LessonUpdateDto, Lesson>()
                .ForMember(d => d.EstimatedMinutes, opt => opt.MapFrom(s => s.Duration))
                .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));

            // Entity -> DTO
            CreateMap<Lesson, LessonDto>()
.ConstructUsing(s => new LessonDto(s.Id, s.CourseId, s.Title, s.Content, s.EstimatedMinutes ));
        }

    }
}
