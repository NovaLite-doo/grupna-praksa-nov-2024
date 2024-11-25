using Konteh.Domain.Enumeration;

namespace Konteh.Domain
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<Answer> Answers { get; set; } = [];
        public QuestionCategory Category { get; set; }
        public QuestionType Type { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
