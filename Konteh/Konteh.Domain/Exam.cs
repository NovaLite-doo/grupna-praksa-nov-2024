﻿namespace Konteh.Domain
{
    public class Exam
    {
        public int Id { get; set; }
        public List<ExamQuestion> Questions { get; } = [];
        public Candidate Candidate { get; set; } = null!;
    }
}
