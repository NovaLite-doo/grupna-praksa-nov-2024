using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    public static class CreateExam
    {
        public const int NumberOfQuestionsPerCategory = 2;
        public class Command : IRequest<Unit>
        {
            public string Email { get; internal set; } = string.Empty;
            public string Faculty { get; internal set; } = string.Empty;
            public string Major { get; internal set; } = string.Empty;
            public string Name { get; internal set; } = string.Empty;
            public string Surname { get; internal set; } = string.Empty;
            public YearOfStudy YearOfStudy { get; internal set; }
        }

        public class RequestHandler : IRequestHandler<Command, Unit>
        {
            private readonly IRepository<Question> _questionRepository;
            private readonly IRepository<Exam> _examRepository;

            public RequestHandler(IRepository<Question> questionRepository, IRepository<Exam> examRepository)
            {
                _questionRepository = questionRepository;
                _examRepository = examRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var questions = await _questionRepository.GetAll();

                var groupedByCategory = questions.GroupBy(q => q.Category).ToList();

                var random = new Random();

                var examQuestions = new List<ExamQuestion>();

                foreach (var categoryGroup in groupedByCategory)
                {
                    var randomQuestions = categoryGroup
                        .OrderBy(x => random.Next())
                        .Take(NumberOfQuestionsPerCategory)
                        .ToList();

                    examQuestions.AddRange(randomQuestions.Select(x => new ExamQuestion
                    {
                        Question = x
                    }));
                }

                var exam = new Exam
                {
                    Candidate = new Candidate
                    {
                        Email = request.Email,
                        Faculty = request.Faculty,
                        Major = request.Major,
                        Name = request.Name,
                        Surname = request.Surname,
                        YearOfStudy = request.YearOfStudy
                    },
                    Questions = examQuestions
                };

                _examRepository.Create(exam);
                await _examRepository.SaveChanges();

                return Unit.Value;
            }
        }

    }
}
