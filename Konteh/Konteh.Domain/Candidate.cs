using Konteh.Domain.Enumeration;

namespace Konteh.Domain
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public YearOfStudy YearOfStudy { get; set; }
        public Exam? Exam { get; set; }
    }
}
