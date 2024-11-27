using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MassTransit.Initializers;
using MediatR;
using System.Collections;
using static Konteh.BackOffice.Api.Featuers.Questions.SearchQuestions;

namespace Konteh.BackOffice.Api.Featuers.Exams
{
    public class GetExams
    {
        public class Query : IRequest<IEnumerable<ExamResponse>>
        {
        }

        public class ExamResponse
        {
            public int Id { get; set; }
            public CandidateResponse candidate { get; set; } = null!;
            public int? QuestionCount { get; set; }
            public int? CorrectAnswerCount { get; set; }
            public double? Score { get; set; }
            public string Status { get; set; } = string.Empty;
        }

        public class CandidateResponse
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Faculty { get; set; } = string.Empty;
            public string Major { get; set; } = string.Empty;
            public YearOfStudy YearOfStudy { get; set; }
        }

        public class RequestHandler : IRequestHandler<Query, IEnumerable<ExamResponse>>
        {
            private readonly IRepository<Exam> _examRepository;

            public RequestHandler(IRepository<Exam> examRepository)
            {
                _examRepository = examRepository;
            }

            public async Task<IEnumerable<ExamResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var exams = await _examRepository.GetAll();

                var response = MapToResponse(exams);

                return response;
            }

            private IEnumerable<ExamResponse> MapToResponse(IEnumerable<Exam> exams)
            {
                return exams.Select(x => new ExamResponse
                {
                    Id = x.Id,
                    QuestionCount = 0,
                    CorrectAnswerCount = 0,
                    Score = 0,
                    Status = "UNKNOWN",
                    candidate = new CandidateResponse 
                    {
                        Name = x.Candidate.Name,
                        Surname = x.Candidate.Surname,
                        Email = x.Candidate.Email,
                        Faculty = x.Candidate.Faculty,
                        Major = x.Candidate.Major,
                        YearOfStudy = x.Candidate.YearOfStudy
                    }
                });
            }
        }
    }
}
