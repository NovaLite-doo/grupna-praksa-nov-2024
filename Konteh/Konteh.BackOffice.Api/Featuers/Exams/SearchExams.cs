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
            public string CandidateName { get; set; } = string.Empty;
            public string Score { get; set; } = string.Empty;
            public ExamStatus Status { get; set; }
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

                var response = exams.Select(MapToResponse);

                return response;
            }

            private async Task<IEnumerable<Exam>> Search(string search) =>
                await _examRepository.Search(x => x.Candidate.Name.Contains(search) || x.Candidate.Surname.Contains(search));

            private ExamResponse MapToResponse(Exam exam) => new()
            {
                Id = exam.Id,
                Status = exam.Status,
                CandidateName = $"{exam.Candidate.Name} {exam.Candidate.Surname}",
                Score = exam.Status == ExamStatus.Completed ? exam.GetScore() : $"0/{exam.Questions.Count}"
            };
        }
    }
}
