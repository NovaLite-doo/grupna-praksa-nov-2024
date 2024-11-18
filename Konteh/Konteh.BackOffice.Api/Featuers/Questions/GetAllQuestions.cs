using Konteh.Domain;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    public static class GetAllQuestions
    {
        public class Query : IRequest<PagedResponse>
        {
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }

        public class PagedResponse
        {
            public IEnumerable<Response> Questions { get; set; } = new List<Response>();
            public int TotalCount { get; set; }
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

        public class RequestHandler : IRequestHandler<Query, PagedResponse>
        {
            private readonly IRepository<Question> _questionRepository;

            public RequestHandler(IRepository<Question> questionRepository)
            {
                _questionRepository = questionRepository;
            }

            public async Task<PagedResponse> Handle(Query request, CancellationToken cancellationToken)
            {

                var allQuestions = await _questionRepository.GetAll();


                var totalCount = allQuestions.Count;


                var questions = allQuestions
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new Response
                    {
                        Id = x.Id,
                        Text = x.Text,
                        Answers = x.Answers.Select(a => new AnswerResponse
                        {
                            Id = a.Id,
                            Text = a.Text
                        })
                    })
                    .ToList();

                return new PagedResponse
                {
                    Questions = questions,
                    TotalCount = totalCount
                };
            }
        }
    }
}
