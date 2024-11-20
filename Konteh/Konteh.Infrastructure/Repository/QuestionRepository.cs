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

        public override async Task<IEnumerable<Question>> GetAll() => await _dbSet.Where(x => !x.IsDeleted).Include(x => x.Answers).ToListAsync();
        public override async Task<Question?> Get(int id) => await _dbSet.Include(x => x.Answers.Where(a => !a.IsDeleted)).FirstOrDefaultAsync(q => q.Id == id);
    }
}

