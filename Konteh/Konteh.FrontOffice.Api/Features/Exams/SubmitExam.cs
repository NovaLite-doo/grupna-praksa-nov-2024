using Konteh.Domain;
using Konteh.Infrastructure.ExceptionHandling;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    public static class SubmitExam
    {
        public class Command : IRequest<Unit>
        {
            public int ExamId { get; set; }
            public List<ExamQuestionDto> ExamQuestions { get; set; } = [];
        }

        public class ExamQuestionDto
        {
            public int Id { get; set; }
            public List<int> SubmittedAnswerIds { get; set; } = [];
        }

        public class RequestHandler : IRequestHandler<Command, Unit>
        {
            private readonly IRepository<Exam> _examRepository;

            public RequestHandler(IRepository<Exam> examRepository)
            {
                _examRepository = examRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var exam = await _examRepository.Get(request.ExamId) ?? throw new EntityNotFoundException();

                foreach (var examQuestion in exam.Questions)
                {
                    var answers = request.ExamQuestions.SingleOrDefault(e => e.Id == examQuestion.Id)?.SubmittedAnswerIds ?? [];

                    examQuestion.SubmittedAnswers = examQuestion.Question.Answers.Where(answer => answers.Contains(answer.Id)).ToList();
                }

                await _examRepository.SaveChanges();

                return Unit.Value;
            }
        }
    }
}
