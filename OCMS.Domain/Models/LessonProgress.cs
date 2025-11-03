using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Domain.Models
{
    public enum LessonStatus { NotStarted = 0, Completed = 1 }

    public class LessonProgress : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = default!;
        public Guid StudentId { get; set; }
        public User Student { get; set; } = default!;
        public LessonStatus Status { get; set; } = LessonStatus.NotStarted;
        public DateTime? CompletedAt { get; set; }
    }
}
