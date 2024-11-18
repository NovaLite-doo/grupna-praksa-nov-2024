using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    public class EditQuestion
    {
        public class QuestionRequest : IRequest<Unit>
        {
            public int Id { get; set; }
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
                var question = await _questionRepository.Get(request.Id);

                if (question == null)
                {
                    throw new KeyNotFoundException($"Question with Id {request.Id} not found.");
                }

                var updatedQuestion = new Question()
                {
                    Text = request.Text,
                    Category = request.Category,
                    Type = request.Type
                };
                var answers = request.Answers.Select(x => new Answer
                {
                    Id = x.Id ?? -1,
                    Text = x.Text,
                    IsCorrect = x.IsCorrect
                });
                updatedQuestion.AddAnswers(answers);

                question.Edit(updatedQuestion);

                await _questionRepository.SaveChanges();

                return Unit.Value;
            }
        }
    }
}
