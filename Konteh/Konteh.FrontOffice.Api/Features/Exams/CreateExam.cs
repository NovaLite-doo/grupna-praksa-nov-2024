using FluentValidation;
using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    public static class CreateExam
    {
        public const int NumberOfQuestionsPerCategory = 2;
        public class Command : IRequest<int>
        {
            public string Email { get; set; } = string.Empty;
            public string Faculty { get; set; } = string.Empty;
            public string Major { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public YearOfStudy YearOfStudy { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private IRepository<Candidate> _candidateRepository;
            public CommandValidator(IRepository<Candidate> candidateRepository)
            {
                _candidateRepository = candidateRepository;
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("A valid email is required.");

                RuleFor(x => x.Email)
                    .MustAsync(NotAlreadyExist)
                    .When(x => !string.IsNullOrEmpty(x.Email))
                    .WithMessage("Email already registered");

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required.");

                RuleFor(x => x.Surname)
                    .NotEmpty().WithMessage("Surname is required.");

                RuleFor(x => x.Faculty)
                    .NotEmpty().WithMessage("Faculty is required.");

                RuleFor(x => x.Major)
                    .NotEmpty().WithMessage("Major is required.");
            }

            private async Task<bool> NotAlreadyExist(string email, CancellationToken cancellationToken) =>
                !(await _candidateRepository.Search(x => x.Email == email)).Any();
        }

        public class RequestHandler : IRequestHandler<Command, int>
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

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
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
                //await _candidateRepository.SaveChanges();

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
                    }));
                }

                _examRepository.Create(exam);
                await _examRepository.SaveChanges();
                return exam.Id;
            }
        }
    }
}
