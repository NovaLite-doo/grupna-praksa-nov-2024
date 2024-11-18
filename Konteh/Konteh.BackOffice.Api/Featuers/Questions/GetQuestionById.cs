using Konteh.Domain;
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
            public IEnumerable<AnswerResponse> Answers { get; set; } = [];
        }
        public class AnswerResponse
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
        }

        public class RequestHandler : IRequestHandler<Query, Response>
        {
            private readonly IRepository<Question> _questionRepository;

            public RequestHandler(IRepository<Question> questionRepository)
            {
                _questionRepository = questionRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var question = await _questionRepository.Get(request.Id);

                if (question == null)
                {
                    throw new KeyNotFoundException($"Question with Id {request.Id} not found.");
                }

                return new Response
                {
                    Id = question.Id,
                    Text = question.Text,
                    Answers = question.Answers.Select(a => new AnswerResponse
                    {
                        Id = a.Id,
                        Text = a.Text
                    })
                };
            }
        }
    }
}
