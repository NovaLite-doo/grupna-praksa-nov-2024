using Konteh.Domain.Enumeration;
using Konteh.Domain;
using Konteh.Infrastructure.Repository;
using MediatR;

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

        public class RequestHandler : IRequestHandler<QuestionRequest, Unit>
        {
            private readonly IRepository<Question> _questionRepository;

            public RequestHandler(IRepository<Question> questionRepository)
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
                var question = await _questionRepository.Get(request.Id ?? -1) ?? throw new KeyNotFoundException($"Question with Id {request.Id} not found.");

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
