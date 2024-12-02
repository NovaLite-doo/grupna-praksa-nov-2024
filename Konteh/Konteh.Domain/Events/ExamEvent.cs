using Konteh.Domain.Enumeration;

namespace Konteh.Domain.Events
{
    public class ExamEvent
    {
        public int Id { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public ExamStatus Status { get; set; }
    }
}
