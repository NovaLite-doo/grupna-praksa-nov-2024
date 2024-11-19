using Konteh.Domain;
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
            private readonly IRepository<Question> _questionRepository;

            public RequestHandler(IRepository<Question> questionRepository)
            {
                _questionRepository = questionRepository;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {

                var question = await _questionRepository.Get(request.Id);
                if (question == null || question.IsDeleted)
                {
                    throw new Exception("Question not found or already deleted.");
                }

                _questionRepository.Delete(question);
                await _questionRepository.SaveChanges();
            }

        }
    }
}

