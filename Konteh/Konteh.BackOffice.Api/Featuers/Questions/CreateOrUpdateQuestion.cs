using Konteh.Domain.Enumeration;
using Konteh.Domain;
using Konteh.Infrastructure.Repository;
using MediatR;
using FluentValidation;
using Konteh.Infrastructure.ExceptionHandling;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    public class CreateOrUpdateQuestion
    {
        public class QuestionRequest : IRequest<Unit>
        {
            public int? Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public QuestionCategory Category { get; set; }
            public QuestionType Type { get; set; }
            public IEnumerable<AnswerRequest> Answers { get; set; } = [];
        }

        public class AnswerRequest
        {
            public int? Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public bool IsCorrect { get; set; }
        }

        public class QuestionValidator : AbstractValidator<QuestionRequest>
        {
            public QuestionValidator()
            {
                RuleFor(x => x.Text).NotEmpty();

                RuleForEach(x => x.Answers).SetValidator(new AnswerValidator());

                RuleFor(x => x)
                    .Must(x => x.Answers.Any(a => a.IsCorrect))
                    .WithMessage("At least one answer must be correct.");

                RuleFor(x => x)
                    .Must(x => x.Answers.Any(a => !a.IsCorrect))
                    .WithMessage("At least one answer must be incorrect.");

                RuleFor(x => x)
                    .Must(x => x.Type != QuestionType.Radiobutton || x.Answers.Count(a => a.IsCorrect) == 1)
                    .WithMessage("Questions of type Radiobutton must have exactly 1 correct answer.");
            }
        }

        public class AnswerValidator : AbstractValidator<AnswerRequest>
        {
            public AnswerValidator()
            {
                RuleFor(x => x.Text).NotEmpty();
            }
        }

        public class RequestHandler : IRequestHandler<QuestionRequest, Unit>
        {
            private readonly IQuestionRepository _questionRepository;

            public RequestHandler(IQuestionRepository questionRepository)
            {
                _questionRepository = questionRepository;
            }

            public async Task<Unit> Handle(QuestionRequest request, CancellationToken cancellationToken)
            {
                if (request.Id == null) 
                {
                    Create(request);
                } 
                else
                {
                    await Edit(request, cancellationToken);
                }

                await _questionRepository.SaveChanges();

                return Unit.Value;
            }

            private void Create(QuestionRequest request)
            {
                var question = new Question
                {
                    Text = request.Text,
                    Category = request.Category,
                    Type = request.Type,
                    Answers = request.Answers.Select(x => new Answer
                    {
                        Text = x.Text,
                        IsCorrect = x.IsCorrect
                    }).ToList()
                };

                _questionRepository.Create(question);
            }

            private async Task Edit(QuestionRequest request, CancellationToken cancellationToken)
            {
                var question = await _questionRepository.Get(request.Id!.Value);
                if (question == null || question.IsDeleted) throw new EntityNotFoundException();

                var updatedQuestion = new Question
                {
                    Text = request.Text,
                    Category = request.Category,
                    Type = request.Type,
                    Answers = request.Answers.Where(x => x.Id != null).Select(x => new Answer
                    {
                        Id = x.Id!.Value,
                        Text = x.Text,
                        IsCorrect = x.IsCorrect
                    }).ToList()
                };

                var newAnswers = request.Answers.Where(x => x.Id == null)
                    .Select(x => new Answer
                    {
                        Text = x.Text,
                        IsCorrect = x.IsCorrect
                    });

                question.Edit(updatedQuestion, newAnswers);
            }
        }
    }
}
