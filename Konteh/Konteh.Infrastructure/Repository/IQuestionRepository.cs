using Konteh.Domain;

namespace Konteh.Infrastructure.Repository
{
    internal interface IQuestionRepository : IRepository<Question>
    {
        public Task<(IEnumerable<Question> questions, int pageCount)> SearchQuestions(
            string searchText,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
    }
}

