using AutoMapper;
using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    public static class CreateQuestion
    {
        public class QuestionRequest : IRequest<Unit>
        {
            public string Text { get; set; } = string.Empty;
            public QuestionCategory Category { get; set; }
            public QuestionType Type { get; set; }
            public IEnumerable<AnswerRequest> Answers { get; set; } = [];
        }

        public class AnswerRequest
        {
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
                var question = new Question()
                {
                    Text = request.Text,
                    Category = request.Category,
                    Type = request.Type
                };
                var answers = request.Answers.Select(x => new Answer
                {
                    Text = x.Text,
                    IsCorrect = x.IsCorrect
                });
                question.AddAnswers(answers);
                question.Validate();

                _questionRepository.Create(question);
                await _questionRepository.SaveChanges();

                return Unit.Value;
            }
        }
    }
}
