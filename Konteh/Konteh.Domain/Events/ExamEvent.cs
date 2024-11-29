namespace Konteh.Domain.Events
{
    public class ExamEvent
    {
        public int Id { get; set; }
        public ExamEventCandidate? Candidate { get; set; }
        public int? QuestionCount { get; set; }
        public int? CorrectAnswerCount { get; set; }
        public double? Score { get; set; }
        public bool IsCompleted { get; set; }
    }
}
