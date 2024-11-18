using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    public static class SearchQuestions
    {
        public class Query : IRequest<PagedResponse>
        {
            public string SearchText { get; set; } = string.Empty;
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;

            public QuestionCategory? Category { get; set; }
        }

        public class PagedResponse
        {
            public IEnumerable<Response> Questions { get; set; } = [];

            public int PageCount { get; set; }


        }


        public class Response
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public QuestionCategory? Category { get; set; }
        }


        public class RequestHandler : IRequestHandler<Query, PagedResponse>
        {
            private readonly IQuestionRepository _questionRepository;

            public RequestHandler(IQuestionRepository questionRepository)
            {
                _questionRepository = questionRepository;
            }

            public async Task<PagedResponse> Handle(Query request, CancellationToken cancellationToken)
            {

                var (questions, pageCount) = await _questionRepository.SearchQuestions(
                    request.SearchText,
                    request.Category,
                    request.PageNumber,
                    request.PageSize,
                    cancellationToken
                );


                var response = new PagedResponse
                {
                    Questions = questions.Select(q => new Response
                    {
                        Id = q.Id,
                        Text = q.Text,
                        Category = q.Category
                    }),
                    PageCount = pageCount
                };

                return response;
            }
        }
    }


}
