using Konteh.Domain;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    public static class GetExam
    {
        public class Query : IRequest<Response>;
        public class Response
        {
            public int Id { get; set; }
            public IEnumerable<ExamQuestionResponse> ExamQuestions { get; set; } = new List<ExamQuestionResponse>();
        }

        public class ExamQuestionResponse
        {
            public int Id { get; set; }
            public int QuestionId { get; set; }
        }

        public class RequestHandler : IRequestHandler<Query, Response>
        {
            private readonly IRepository<Question> _questionRepository;
            private readonly IRepository<Exam> _examRepository;

            public RequestHandler(IRepository<Question> questionRepository, IRepository<Exam> examRepository)
            {
                _questionRepository = questionRepository;
                _examRepository = examRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var questions = await _questionRepository.GetAll();

                var groupedByCategory = questions.GroupBy(q => q.Category).ToList();

                var selectedQuestions = new List<Response>();

                var random = new Random();

                var examQuestions = new List<ExamQuestion>();

                foreach (var categoryGroup in groupedByCategory)
                {
                    var randomQuestions = categoryGroup
                        .OrderBy(x => random.Next())
                        .Take(2)
                        .ToList();

                    foreach (var question in randomQuestions)
                    {
                        var examQuestion = new ExamQuestion
                        {
                            QuestionId = question.Id,
                            Question = question
                        };

                        examQuestions.Add(examQuestion);
                    }

                }

                var examQuestionResponses = examQuestions.Select(eq => new ExamQuestionResponse
                {
                    QuestionId = eq.QuestionId,
                    Id = eq.Id
                }).ToList();


                var response = new Response
                {
                    ExamQuestions = examQuestionResponses
                };

                return response;
            }
        }

    }
}
