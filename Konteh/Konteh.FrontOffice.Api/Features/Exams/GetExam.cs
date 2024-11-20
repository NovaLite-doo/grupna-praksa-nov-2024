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
            public IEnumerable<AnswerResponse> Answers { get; set; } = new List<AnswerResponse>();
        }

        public class ExamQuestionResponse
        {
            public int Id { get; set; }
            public string QuestionText { get; set; } = string.Empty;
            public List<AnswerResponse> Answers { get; set; } = new List<AnswerResponse>();
        }

        public class AnswerResponse
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
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

                var random = new Random();

                var examQuestions = new List<ExamQuestionResponse>();

                foreach (var categoryGroup in groupedByCategory)
                {
                    var randomQuestions = categoryGroup
                        .OrderBy(x => random.Next())
                        .Take(2)
                        .ToList();

                    foreach (var question in randomQuestions)
                    {
                        var answerResponses = question.Answers
                            .OrderBy(a => random.Next())
                            .Select(a => new AnswerResponse
                            {
                                Id = a.Id,
                                Text = a.Text
                            })
                            .ToList();

                        var examQuestionResponse = new ExamQuestionResponse
                        {
                            Id = question.Id,
                            QuestionText = question.Text,  
                            Answers = answerResponses
                        };

                        examQuestions.Add(examQuestionResponse);
                    }

                }

                var response = new Response
                {
                    ExamQuestions = examQuestions
                };

                return response;
            }
        }

    }
}
