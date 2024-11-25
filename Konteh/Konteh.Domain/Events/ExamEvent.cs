using Konteh.Domain.Enumeration;

namespace Konteh.Domain.Events
{
    public class ExamEvent
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public ExamEventType Type { get; set; }
    }
}
