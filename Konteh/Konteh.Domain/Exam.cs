using Konteh.Domain.Enumeration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Konteh.Domain
{
    public class Exam
    {
        public int Id { get; set; }
        public List<ExamQuestion> Questions { get; set; } = [];
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; } = null!;
        public ExamStatus Status { get; set; }

        public string GetScore()
        {
            var correctAnswers = Questions.Count(x => x.IsCorrect());
            return $"{correctAnswers}/{Questions.Count}";
        }
    }
}
