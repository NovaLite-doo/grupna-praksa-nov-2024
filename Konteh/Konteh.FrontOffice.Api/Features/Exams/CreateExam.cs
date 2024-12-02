using FluentValidation;
using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Domain.Events;
using Konteh.Infrastructure;
using Konteh.Infrastructure.Repository;
using MassTransit;
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
            private readonly IPublishEndpoint _publishEndpoint;
            private readonly IRandom _random;

            public RequestHandler(IRepository<Question> questionRepository, IRepository<Exam> examRepository, IRandom random, IRepository<Candidate> candidateRepository, IPublishEndpoint publishEndpoint)
            {
                _questionRepository = questionRepository;
                _examRepository = examRepository;
                _random = random;
                _candidateRepository = candidateRepository;
                _publishEndpoint = publishEndpoint;
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

                var exam = new Exam
                {
                    Candidate = candidate,
                    Status = ExamStatus.InProgress,
                    DateTimeStarted = DateTime.Now
                };

                foreach (var categoryGroup in groupedByCategory)
                {
                    var randomQuestions = categoryGroup
                        .OrderBy(x => _random.NextInt())
                        .Take(NumberOfQuestionsPerCategory)
                        .ToList();

                    exam.Questions.AddRange(randomQuestions.Select(x => new ExamQuestion
                    {
                        Question = x
                    }));
                }

                _examRepository.Create(exam);
                await _examRepository.SaveChanges();

                SendNotification(exam);

                return exam.Id;
            }

            private void SendNotification(Exam exam)
            {
                _publishEndpoint.Publish(new ExamEvent
                {
                    Id = exam.Id,
                    Status = exam.Status,
                    CandidateName = $"{exam.Candidate.Name} {exam.Candidate.Surname}",
                    Score = $"0/{exam.Questions.Count}"
                });
            }
        }
    }
}
