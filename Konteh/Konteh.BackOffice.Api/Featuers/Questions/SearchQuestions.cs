using Konteh.Domain.Enumeration;
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
            public QuestionCategory Category { get; set; }
        }




    }
}
