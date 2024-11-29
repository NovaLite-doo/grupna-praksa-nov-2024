using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    public static class CreateExam
    {
        public const int NumberOfQuestionsPerCategory = 1;
        public class Command : IRequest<int>
        {
            public string Email { get; set; } = string.Empty;
            public string Faculty { get; set; } = string.Empty;
            public string Major { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public YearOfStudy YearOfStudy { get; set; }
        }

        public class RequestHandler : IRequestHandler<Command, int>
        {
            private readonly IRepository<Question> _questionRepository;
            private readonly IRepository<Exam> _examRepository;
            private readonly IRepository<Candidate> _candidateRepository;
            private readonly Random _random;

            public RequestHandler(IRepository<Question> questionRepository, IRepository<Exam> examRepository, Random random, IRepository<Candidate> candidateRepository)
            {
                _questionRepository = questionRepository;
                _examRepository = examRepository;
                _random = random;
                _candidateRepository = candidateRepository;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var questions = await _questionRepository.GetAll();

                var groupedByCategory = questions.GroupBy(q => q.Category).ToList();

                var examQuestions = new List<ExamQuestion>();

                var candidate = new Candidate
                {
                    Email = request.Email,
                    Faculty = request.Faculty,
                    Major = request.Major,
                    Name = request.Name,
                    Surname = request.Surname,
                    YearOfStudy = request.YearOfStudy
                };
                _candidateRepository.Create(candidate);
                //await _candidateRepository.SaveChanges();

                var exam = new Exam
                {
                    Candidate = candidate,
                    Questions = examQuestions
                };

                foreach (var categoryGroup in groupedByCategory)
                {
                    var randomQuestions = categoryGroup
                        .OrderBy(x => _random.Next())
                        .Take(NumberOfQuestionsPerCategory)
                        .ToList();

                    examQuestions.AddRange(randomQuestions.Select(x => new ExamQuestion
                    {
                        Question = x
                    }));
                }

                _examRepository.Create(exam);
                await _examRepository.SaveChanges();

                return exam.Id;
            }
        }

    }
}
