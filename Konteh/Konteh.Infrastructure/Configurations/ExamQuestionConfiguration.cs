using Konteh.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Konteh.Infrastructure.Configurations
{
    public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamQuestion> builder)
        {

            builder
                .HasMany(e => e.SubmittedAnswers)
                .WithMany()
                .UsingEntity<ExamQuestionAnswer>(
                l => l.HasOne<Answer>().WithMany().OnDelete(DeleteBehavior.NoAction),
                r => r.HasOne<ExamQuestion>().WithMany().OnDelete(DeleteBehavior.NoAction));

            builder
                .HasOne(e => e.Question)
                .WithMany()
                .HasForeignKey(x => x.QuestionId);
        }
    }
}
