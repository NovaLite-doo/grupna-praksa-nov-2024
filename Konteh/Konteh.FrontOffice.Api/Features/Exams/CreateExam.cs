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
            public CommandValidator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("A valid email is required.");

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required.");

                RuleFor(x => x.Surname)
                    .NotEmpty().WithMessage("Surname is required.");

                RuleFor(x => x.Faculty)
                    .NotEmpty().WithMessage("Faculty is required.");

                RuleFor(x => x.Major)
                    .NotEmpty().WithMessage("Major is required.");
            }
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
                await EnsureCandidateHasNotTakenExam(request.Email);

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
                    }));
                }

                _examRepository.Create(exam);
                await _examRepository.SaveChanges();
                return exam.Id;
            }

            private async Task EnsureCandidateHasNotTakenExam(string email)
            {
                var existingCandidate = (await _candidateRepository.Search(c => c.Email == email)).FirstOrDefault();

                if (existingCandidate != null)
                {
                    var existingExams = await _examRepository.Search(e => e.CandidateId == existingCandidate.Id);

                    if (existingExams.Any())
                    {
                        throw new ValidationException("Candidate has already taken the exam.");
                    }
                }
            }
        }

    }
}
