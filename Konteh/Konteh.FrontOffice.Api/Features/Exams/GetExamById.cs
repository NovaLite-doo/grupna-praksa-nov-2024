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
            public int Id { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public List<ExamQuestion> Questions { get; set; } = [];
        }
        public class ExamQuestionDto
        {
            public int Id { get; set; }
            public string QuestionText { get; set; } = string.Empty;
            public QuestionCategory Category { get; set; }
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
                var exam = await _examRepository.Get(request.Id);
                if (exam == null)
                {
                    throw new KeyNotFoundException($"Exam with ID {request.Id} not found.");
                }

                var response = new Response
                {
                    Id = exam.Id,
                    Questions = exam.Questions
                };

                return response;
            }
        }
    }
}
