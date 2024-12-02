using Konteh.Domain.Enumeration;

namespace Konteh.Domain.Events
{
    public class ExamEvent
    {
        public int Id { get; set; }
        public ExamEventCandidate Candidate { get; set; } = new ExamEventCandidate();
        public string? Score { get; set; }
        public ExamStatus Status { get; set; }
    }
}
