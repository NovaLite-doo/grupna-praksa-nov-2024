namespace Konteh.Domain
{
    public class ExamQuestion
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public List<Answer> SubmittedAnswers { get; } = [];
        public int ExamId { get; set; }
        public Exam Exam { get; set; } = null!;

        public bool IsCorrect()
        {
            var correctAnswers = Question.Answers.Where(x => x.IsCorrect);

            return correctAnswers.Count() == SubmittedAnswers.Count 
                && !correctAnswers.Any(x => !SubmittedAnswers.Contains(x));
        }
    }
}
