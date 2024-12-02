using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Konteh.Infrastructure.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasMany(x => x.Answers).WithOne(x => x.Question).HasForeignKey(x => x.QuestionId);
            builder.HasDiscriminator(x => x.Type)
                .HasValue<CheckboxQuestion>(QuestionType.Checkbox)
                .HasValue<RadioButtonQuestion>(QuestionType.Radiobutton);
        }
    }
}
