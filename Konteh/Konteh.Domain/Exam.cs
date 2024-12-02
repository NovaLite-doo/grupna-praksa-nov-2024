namespace Konteh.Domain
{
    public class Exam
    {
        public int Id { get; set; }
        public List<ExamQuestion> Questions { get; set; } = [];
        public Candidate Candidate { get; set; } = null!;
        public int CandidateId { get; set; }
    }
}