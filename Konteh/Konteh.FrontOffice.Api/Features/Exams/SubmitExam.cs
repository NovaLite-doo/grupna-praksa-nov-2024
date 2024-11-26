using Konteh.Domain;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    public static class SubmitExam
    {
        public class Command : IRequest<Unit>
        {
            public int ExamId { get; set; }
            public List<ExamQuestionDTO> ExamQuestions { get; set; } = new List<ExamQuestionDTO>();
        }

        public class ExamQuestionDTO
        {
            public int Id { get; set; }
            public List<int> SubmittedAnswers { get; set; } = [];
        }

        public class RequestHandler : IRequestHandler<Command, Unit>
        {
            private readonly IRepository<Exam> _examRepository;
            private readonly IRepository<Question> _questionRepository;
            private readonly IRepository<Candidate> _candidateRepository;

            public RequestHandler(IRepository<Exam> examRepository, IRepository<Question> questionRepository, IRepository<Candidate> candidateRepository)
            {
                _examRepository = examRepository;
                _questionRepository = questionRepository;
                _candidateRepository = candidateRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var exam = await _examRepository.Get(request.ExamId);

                if (exam == null)
                {
                    throw new InvalidOperationException("Exam not found.");
                }

                foreach (var examQuestion in exam.Questions)
                {
                    var answers = request.ExamQuestions.Single(e => e.Id == examQuestion.Id).SubmittedAnswers;

                    examQuestion.SubmittedAnswers = examQuestion.Question.Answers.Where(answer => answers.Contains(answer.Id)).ToList();

                }

                await _examRepository.SaveChanges();

                return Unit.Value;
            }
        }
    }

}

