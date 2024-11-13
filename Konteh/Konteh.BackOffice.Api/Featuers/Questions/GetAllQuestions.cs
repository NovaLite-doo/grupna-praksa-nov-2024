using Konteh.Domain;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    public static class GetAllQuestions
    {
        public class Query : IRequest<IEnumerable<Response>>;
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

        public class RequestHandler : IRequestHandler<Query, IEnumerable<Response>>
        {
            private readonly IRepository<Question> _questionRepository;

            public RequestHandler(IRepository<Question> questionRepository)
            {
                _questionRepository = questionRepository;
            }

            public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var questions = await _questionRepository.GetAll();

                return questions.Select(x => new Response
                {
                    Id = x.Id,
                    Text = x.Text,
                    Answers = x.Answers.Select(a => new AnswerResponse
                    {
                        Id = a.Id,
                        Text = a.Text
                    })
                });
            }
        }
    }
}
