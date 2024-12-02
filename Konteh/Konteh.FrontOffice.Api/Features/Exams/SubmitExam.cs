using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.Domain.Events;
using Konteh.Infrastructure.ExceptionHandling;
using Konteh.Infrastructure.Repository;
using MassTransit;
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
            public List<int> SubmittedAnswers { get; set; } = [];
        }

        public class RequestHandler : IRequestHandler<Command, Unit>
        {
            private readonly IRepository<Exam> _examRepository;
            private readonly IPublishEndpoint _publishEndpoint;

            public RequestHandler(IRepository<Exam> examRepository, IPublishEndpoint publishEndpoint)
            {
                _examRepository = examRepository;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var exam = await _examRepository.Get(request.ExamId) ?? throw new EntityNotFoundException();

                foreach (var examQuestion in exam.Questions)
                {
                    var answers = request.ExamQuestions.SingleOrDefault(e => e.Id == examQuestion.Id)?.SubmittedAnswers ?? [];

                    examQuestion.SubmittedAnswers = examQuestion.Question.Answers.Where(answer => answers.Contains(answer.Id)).ToList();
                }

                exam.Status = ExamStatus.Completed;

                await _examRepository.SaveChanges();

                SendNotification(exam);

                return Unit.Value;
            }

            private void SendNotification(Exam exam)
            {
                _publishEndpoint.Publish(new ExamEvent
                {
                    Id = exam.Id,
                    Status = exam.Status,
                    Score = exam.GetScore()
                });
            }
        }
    }
}
