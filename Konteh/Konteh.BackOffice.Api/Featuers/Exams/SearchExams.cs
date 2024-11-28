using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MassTransit.Initializers;
using MediatR;
using System.Linq.Expressions;

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
            public CandidateResponse candidate { get; set; } = null!;
            public int? QuestionCount { get; set; }
            public int? CorrectAnswerCount { get; set; }
            public double? Score { get; set; }
            public bool IsCompleted { get; set; }
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
                var exams = string.IsNullOrEmpty(request.Search) ?
                    await _examRepository.GetAll() :
                    await Search(request.Search);

                var response = exams.Select(x => MapToResponse(x));

                return response;
            }

            private async Task<IEnumerable<Exam>> Search(string search)
            {
                search = search.ToLower().Replace(" ", "");

                Expression<Func<Exam, bool>> predicate = x => (x.Candidate.Name + x.Candidate.Surname).ToLower().Contains(search);

                return await _examRepository.Search(predicate);
            }

            private ExamResponse MapToResponse(Exam exam)
            {
                var response = new ExamResponse
                {
                    Id = exam.Id,
                    IsCompleted = exam.IsCompleted,
                    candidate = new CandidateResponse
                    {
                        Name = exam.Candidate.Name,
                        Surname = exam.Candidate.Surname,
                        Email = exam.Candidate.Email,
                        Faculty = exam.Candidate.Faculty,
                        Major = exam.Candidate.Major,
                        YearOfStudy = exam.Candidate.YearOfStudy
                    }
                };

                if(exam.IsCompleted)
                {
                    response.QuestionCount = exam.Questions.Count;
                    response.CorrectAnswerCount = exam.GetCorrectAnswerCount();
                    response.Score = exam.GetScore();
                }

                return response;
            }
        }
    }
}
