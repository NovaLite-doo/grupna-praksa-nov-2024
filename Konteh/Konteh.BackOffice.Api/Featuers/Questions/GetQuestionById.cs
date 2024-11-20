using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    public class GetQuestionById
    {
        public class Query : IRequest<Response>
        {
            public int Id { get; set; }
        }
        public class Response
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public QuestionCategory Category { get; set; }
            public QuestionType Type { get; set; }
            public IEnumerable<AnswerResponse> Answers { get; set; } = [];
        }
        public class AnswerResponse
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public bool IsCorrect { get; set; }
        }

        public class RequestHandler : IRequestHandler<Query, Response>
        {
            private readonly IQuestionRepository _questionRepository;

            public RequestHandler(IQuestionRepository questionRepository)
            {
                _questionRepository = questionRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var question = await _questionRepository.Get(request.Id) ?? throw new KeyNotFoundException($"Question with Id {request.Id} not found.");
                return new Response
                {
                    Id = question.Id,
                    Text = question.Text,
                    Category = question.Category,
                    Type = question.Type,
                    Answers = question.Answers.Select(a => new AnswerResponse
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    })
                };
            }
        }
    }
}
