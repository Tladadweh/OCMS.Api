using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Domain.Models
{
    public class Course : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool IsPublished { get; set; }
        public Guid InstructorId { get; set; }
        public User Instructor { get; set; } = default!;
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
