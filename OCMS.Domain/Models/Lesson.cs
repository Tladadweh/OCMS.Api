using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Domain.Models
{
    public class Lesson : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Content { get; set; }   
        public int EstimatedMinutes { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = default!;
        public ICollection<LessonProgress> ProgressEntries { get; set; } = new List<LessonProgress>();

    }
}
