using Microsoft.EntityFrameworkCore;
using OCMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<LessonProgress> LessonProgresses => Set<LessonProgress>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Course>().HasIndex(x => x.Code).IsUnique();

            b.Entity<Enrollment>().HasIndex(e => new { e.CourseId, e.StudentId }).IsUnique();
            b.Entity<LessonProgress>().HasIndex(p => new { p.LessonId, p.StudentId }).IsUnique();

            b.Entity<Course>()
              .HasOne(c => c.Instructor).WithMany(u => u.CoursesTaught)
              .HasForeignKey(c => c.InstructorId).OnDelete(DeleteBehavior.Restrict);

            b.Entity<Lesson>()
              .HasOne(l => l.Course).WithMany(c => c.Lessons)
              .HasForeignKey(l => l.CourseId);

            b.Entity<Enrollment>()
              .HasOne(e => e.Course).WithMany(c => c.Enrollments)
              .HasForeignKey(e => e.CourseId);

            b.Entity<Enrollment>()
              .HasOne(e => e.Student).WithMany(u => u.Enrollments)
              .HasForeignKey(e => e.StudentId);

            b.Entity<LessonProgress>()
              .HasOne(p => p.Lesson).WithMany(l => l.ProgressEntries)
              .HasForeignKey(p => p.LessonId);

            b.Entity<LessonProgress>()
              .HasOne(p => p.Student).WithMany()
              .HasForeignKey(p => p.StudentId);
        }
    }
}