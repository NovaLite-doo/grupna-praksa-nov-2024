using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    public static class CreateExam
    {
        public const int NumberOfQuestionsPerCategory = 2;
        public class Command : IRequest<Exam>
        {
            public string Email { get; set; } = string.Empty;
            public string Faculty { get; set; } = string.Empty;
            public string Major { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public YearOfStudy YearOfStudy { get; set; }
        }

        public class RequestHandler : IRequestHandler<Command, Exam>
        {
            private readonly IRepository<Question> _questionRepository;
            private readonly IRepository<Exam> _examRepository;
            private readonly IRepository<Candidate> _candidateRepository;

            public RequestHandler(IRepository<Question> questionRepository, IRepository<Exam> examRepository, IRepository<Candidate> candidateRepository)
            {
                _questionRepository = questionRepository;
                _examRepository = examRepository;
                _candidateRepository = candidateRepository;
            }

            public async Task<Exam> Handle(Command request, CancellationToken cancellationToken)
            {

                var questions = await _questionRepository.GetAll();

                var groupedByCategory = questions.GroupBy(q => q.Category).ToList();

                var random = new Random();

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
                await _candidateRepository.SaveChanges();


                var exam = new Exam
                {
                    Candidate = candidate
                };

                foreach (var categoryGroup in groupedByCategory)
                {
                    var randomQuestions = categoryGroup
                        .OrderBy(x => random.Next())
                        .Take(NumberOfQuestionsPerCategory)
                        .ToList();

                    exam.Questions.AddRange(randomQuestions.Select(x => new ExamQuestion
                    {
                        Question = x
                        //QuestionId = x.Id
                        //Exam = exam
                    }));
                }

                //exam.Questions = examQuestions;

                _examRepository.Create(exam);
                await _examRepository.SaveChanges();
                return exam;
            }
        }

    }
}
