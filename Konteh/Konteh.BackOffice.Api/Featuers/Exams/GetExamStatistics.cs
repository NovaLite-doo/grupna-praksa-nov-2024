using Konteh.Domain;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Exams
{
    public class GetExamStatistics
    {
        public class Query : IRequest<ExamStatisticsResponse> { }

        public class Handler : IRequestHandler<Query, ExamStatisticsResponse>
        {
            private readonly IRepository<Exam> _examRepository;

            public Handler(IRepository<Exam> examRepository)
            {
                _examRepository = examRepository;
            }

            public async Task<ExamStatisticsResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var exams = await _examRepository.GetAll();
                int totalExams = exams.Count();
                int above50 = exams.Count(e => CalculateScore(e) > 50);
                int below50 = totalExams - above50;

                return new ExamStatisticsResponse
                {
                    TotalExams = totalExams,
                    Above50Percent = above50,
                    Below50Percent = below50
                };
            }

            public double CalculateScore(Exam exam)
            {
                int totalCorrectAnswers = 0;
                int totalQuestions = exam.Questions.Count;

                foreach (var examQuestion in exam.Questions)
                {
                    var correctAnswers = examQuestion.Question.Answers.Where(a => a.IsCorrect).ToList();
                    if (correctAnswers.All(correctAnswer => examQuestion.SubmittedAnswers.Contains(correctAnswer)))
                    {
                        totalCorrectAnswers++;
                    }
                }

                return (double)totalCorrectAnswers / totalQuestions * 100;
            }

        }

        public class ExamStatisticsResponse
        {
            public int TotalExams { get; set; }
            public int Above50Percent { get; set; }
            public int Below50Percent { get; set; }
        }
    }
}
