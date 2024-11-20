using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Microsoft.EntityFrameworkCore;

namespace Konteh.Infrastructure.Repository
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IEnumerable<Question>> GetAll()
        {
            return await _dbSet
                .Where(q => !q.IsDeleted)
                .ToListAsync();
        }

        public override void Delete(Question entity)
        {
            entity.IsDeleted = true;
        }

        public async Task<(IEnumerable<Question> questions, int totalItems)> SearchQuestions(
            string? searchText,
            QuestionCategory? category,
            int pageNumber,
            int pageSize)
        {
            var query = _dbSet.AsQueryable().Where(q => !q.IsDeleted);

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(q => q.Text.Contains(searchText));
            }

            if (category != null)
            {
                query = query.Where(q => q.Category == category);
            }

            var totalCount = await query.CountAsync();
            var questions = await query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .Include(q => q.Answers)
                .ToListAsync();

            return (questions, totalCount);
        }


    }
}

