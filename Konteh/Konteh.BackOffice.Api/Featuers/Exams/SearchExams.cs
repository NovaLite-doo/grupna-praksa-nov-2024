using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MassTransit.Initializers;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Exams
{
    public class SearchExams
    {
        public class Query : IRequest<IEnumerable<ExamResponse>>
        {
            public string? Search { get; set; } 
        }

        public class ExamResponse
        {
            public int Id { get; set; }
            public CandidateResponse Candidate { get; set; } = new CandidateResponse();
            public string? Score { get; set; }
            public ExamStatus Status { get; set; }
        }

        public class CandidateResponse
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
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
                var exams = string.IsNullOrEmpty(request.Search) ?
                    await _examRepository.GetAll() :
                    await Search(request.Search);

                var response = exams.Reverse().Select(x => MapToResponse(x));

                return response;
            }

            private async Task<IEnumerable<Exam>> Search(string search)
            {
                search = search.ToLower().Replace(" ", "");

                return await _examRepository.Search(x =>
                    (x.Candidate.Name + x.Candidate.Surname).ToLower().Contains(search)
                );
            }

            private ExamResponse MapToResponse(Exam exam)
            {
                var response = new ExamResponse
                {
                    Id = exam.Id,
                    Status = exam.Status,
                    Candidate = new CandidateResponse
                    {
                        Name = exam.Candidate.Name,
                        Surname = exam.Candidate.Surname
                    }
                };

                if(exam.Status == ExamStatus.Completed)
                {
                    response.Score = exam.GetScore();
                }

                return response;
            }
        }
    }
}
