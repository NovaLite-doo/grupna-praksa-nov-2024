using Konteh.Infrastructure.ExceptionHandling;
using Konteh.Infrastructure.Repository;
using MediatR;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    public static class DeleteQuestion
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class RequestHandler : IRequestHandler<Command>
        {
            private readonly IQuestionRepository _questionRepository;

            public RequestHandler(IQuestionRepository questionRepository)
            {
                _questionRepository = questionRepository;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {

                var question = await _questionRepository.Get(request.Id);
                if (question == null || question.IsDeleted) throw new EntityNotFoundException($"Question with Id {request.Id} not found.");

                _questionRepository.Delete(question);
                await _questionRepository.SaveChanges();
            }
        }
    }
}

