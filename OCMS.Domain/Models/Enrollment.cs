using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Domain.Models
{
    public class Enrollment : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = default!;
        public Guid StudentId { get; set; }
        public User Student { get; set; } = default!;

    }
}
