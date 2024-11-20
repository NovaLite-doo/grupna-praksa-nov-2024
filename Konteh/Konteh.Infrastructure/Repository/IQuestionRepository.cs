using Konteh.Domain;
using Konteh.Domain.Enumeration;

namespace Konteh.Infrastructure.Repository
{
    public interface IQuestionRepository : IRepository<Question>
    {
        public Task<(IEnumerable<Question> questions, int totalItems)> SearchQuestions(
            string? searchText,
            QuestionCategory? category,
            int pageNumber,
            int pageSize);
    }
}

