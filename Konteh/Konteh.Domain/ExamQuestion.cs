namespace Konteh.Domain
{
    public class ExamQuestion
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public List<Answer> SubmittedAnswers { get; set; } = [];
        public int ExamId { get; set; }
        public Exam Exam { get; set; } = null!;
    }
}
