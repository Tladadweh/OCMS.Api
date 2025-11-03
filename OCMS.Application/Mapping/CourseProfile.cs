using AutoMapper;
using OCMS.Application.DTO;
using OCMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OCMS.Application.DTO.CourseDtos;

namespace OCMS.Application.Mapping
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseCreateDto, Course>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.InstructorId, opt => opt.Ignore())
                .ForMember(d => d.IsPublished, opt => opt.MapFrom(_ => false));

            CreateMap<Course, CourseDetailsDto>();
            CreateMap<CourseUpdateDto, Course>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Course, CourseListItemDto>();

            CreateMap<ValueTuple<Course, int>, CourseTopItemDto>()
                .ForCtorParam("Id", o => o.MapFrom(s => s.Item1.Id))
                .ForCtorParam("Title", o => o.MapFrom(s => s.Item1.Title))
                .ForCtorParam("Category", o => o.MapFrom(s => s.Item1.Category))
                .ForCtorParam("EnrollmentCount", o => o.MapFrom(s => s.Item2));


        }

    }

}

