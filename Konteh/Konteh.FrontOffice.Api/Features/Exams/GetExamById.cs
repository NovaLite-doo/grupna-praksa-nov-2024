using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    public class GetExamById
    {
        public class Query : IRequest<Response>
        {
            public int ExamId { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public List<ExamQuestionDto> Questions { get; set; } = [];
        }
        public class ExamQuestionDto
        {
            public int Id { get; set; }

            public int ExamId { get; set; }
            public int QuestionId { get; set; }
            public string Text { get; set; } = string.Empty;
            public QuestionType Type { get; set; }
            public IEnumerable<AnswerDto> Answers { get; set; } = [];

        }
        public class AnswerDto
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public bool IsCorrect { get; set; }
        }
        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IRepository<Exam> _examRepository;

            public Handler(IRepository<Exam> examRepository)
            {
                _examRepository = examRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var exam = await _examRepository.Get(request.ExamId);
                if (exam == null)
                {
                    throw new KeyNotFoundException($"Exam with ID {request.ExamId} not found.");
                }

                var response = new Response
                {
                    Id = exam.Id,
                    Questions = exam.Questions.Select(q => new ExamQuestionDto
                    {
                        Id = q.Id,
                        ExamId = q.ExamId,
                        QuestionId = q.QuestionId,
                        Text = q.Question.Text,
                        Type = q.Question.Type,
                        Answers = q.Question.Answers.Select(a => new AnswerDto
                        {
                            Id = a.Id,
                            Text = a.Text
                        }).ToList()
                    }).ToList()
                };

                return response;
            }
        }
    }
}