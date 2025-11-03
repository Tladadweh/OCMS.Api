using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Domain.Models
{
    public class User : AuditableEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = "Student"; // Admin/Instructor/Student
        public string? FullName { get; set; }
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }
        public ICollection<Course> CoursesTaught { get; set; } = new List<Course>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}

