namespace Konteh.Domain
{
    public class Exam
    {
        public int Id { get; set; }
        public List<ExamQuestion> Questions { get; set; } = [];
        public Candidate Candidate { get; set; } = null!;
        public bool IsCompleted { get; set; }

        public int GetCorrectAnswerCount()
        {
            return Questions.Count(x => x.IsCorrect());
        }

        public double GetScore()
        {
            double score = (double)GetCorrectAnswerCount() / Questions.Count * 100;
            return Math.Round(score, 2);
        }
    }
}
