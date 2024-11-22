using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Infrastructure.Repository;
using MediatR;
using System;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    public static class CreateExam
    {
        public const int NumberOfQuestionsPerCategory = 1;
        public class Command : IRequest<Exam>
        {
            public string Email { get; set; } = string.Empty;
            public string Faculty { get; set; } = string.Empty;
            public string Major { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public YearOfStudy YearOfStudy { get; set; }
        }

        public class RequestHandler : IRequestHandler<Command, Exam>
        {
            private readonly IRepository<Question> _questionRepository;
            private readonly IRepository<Exam> _examRepository;
            private readonly Random _random;

            public RequestHandler(IRepository<Question> questionRepository, IRepository<Exam> examRepository, Random random)
            {
                _questionRepository = questionRepository;
                _examRepository = examRepository;
                _random = random;
            }

            public async Task<Exam> Handle(Command request, CancellationToken cancellationToken)
            {
                var questions = await _questionRepository.GetAll();

                var groupedByCategory = questions.GroupBy(q => q.Category).ToList();

                var examQuestions = new List<ExamQuestion>();

                var exam = new Exam
                {
                    Candidate = new Candidate
                    {
                        Email = request.Email,
                        Faculty = request.Faculty,
                        Major = request.Major,
                        Name = request.Name,
                        Surname = request.Surname,
                        YearOfStudy = request.YearOfStudy
                    },
                    Questions = examQuestions
                };

                foreach (var categoryGroup in groupedByCategory)
                {
                    var randomQuestions = categoryGroup
                        .OrderBy(x => _random.Next())
                        .Take(NumberOfQuestionsPerCategory)
                        .ToList();

                    examQuestions.AddRange(randomQuestions.Select(x => new ExamQuestion
                    {
                        Question = x,
                        QuestionId = x.Id,
                        ExamId = exam.Id
                    }));
                }  

                _examRepository.Create(exam);
                await _examRepository.SaveChanges();

                return exam;
            }
        }

    }
}
