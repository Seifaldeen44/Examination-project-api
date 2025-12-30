using examProj.Dto;
using examProj.Models;
using Microsoft.EntityFrameworkCore;

namespace examProj.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options) { }

        public DbSet<Student> Student { get; set; }
        public DbSet<Instructor> Instructor { get; set; }

        public DbSet<Stud_Course> Stud_Courses { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<ExamQuestionDto> ExamQuestions { get; set; }

        public DbSet<ExamResultDto> ExamResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stud_Course>()
                .HasKey(sc => new { sc.St_ID, sc.Crs_ID });

            modelBuilder.Entity<ExamQuestionDto>().HasNoKey();
            modelBuilder.Entity<ExamResultDto>().HasNoKey();
        }



    }
}
