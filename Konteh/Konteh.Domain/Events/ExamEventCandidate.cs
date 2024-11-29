using Konteh.Domain.Enumeration;

namespace Konteh.Domain.Events
{
    public class ExamEventCandidate
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public YearOfStudy YearOfStudy { get; set; }
    }
}
