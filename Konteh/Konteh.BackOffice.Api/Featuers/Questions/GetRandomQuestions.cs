using Konteh.Domain;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    public static class GetRandomQuestions
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

                var groupedByCategory = questions.GroupBy(q => q.Category).ToList();

                var selectedQuestions = new List<Response>();

                var random = new Random();

                foreach (var categoryGroup in groupedByCategory)
                {
                    var randomQuestions = categoryGroup
                        .OrderBy(x => random.Next()) 
                        .Take(2) 
                        .ToList();

                    selectedQuestions.AddRange(randomQuestions.Select(x => new Response
                    {
                        Id = x.Id,
                        Text = x.Text
                    }));
                }

                return selectedQuestions;
            }
        }
    }
}
