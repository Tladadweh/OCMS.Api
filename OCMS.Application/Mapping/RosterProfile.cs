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
    public class RosterProfile : Profile
    {
        public RosterProfile()
        {
            CreateMap<Enrollment, RosterStudentDto>()
                .ForCtorParam("StudentId", o => o.MapFrom(e => e.StudentId))
                .ForCtorParam("Name", o => o.MapFrom(e => e.Student.UserName))
                .ForCtorParam("Email", o => o.MapFrom(e => e.Student.Email))
                .ForCtorParam("EnrolledAtUtc", o => o.MapFrom(e => e.CreatedAt.ToUniversalTime()));
        }

    }
}
