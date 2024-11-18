using Konteh.Domain.Enumeration;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    public static class GetCategories
    {
        public class Query : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
        }

        public class CategoryResponse
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class RequestHandler : IRequestHandler<Query, Response>
        {
            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {

                var categories = Enum.GetValues(typeof(QuestionCategory))
                    .Cast<QuestionCategory>()
                    .Select(c => new CategoryResponse
                    {
                        Id = (int)c,
                        Name = c.ToString()
                    })
                    .ToList();

                return Task.FromResult(new Response
                {
                    Categories = categories
                });
            }
        }
    }

}

