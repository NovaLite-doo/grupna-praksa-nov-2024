using Konteh.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Konteh.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<Question> Questions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
