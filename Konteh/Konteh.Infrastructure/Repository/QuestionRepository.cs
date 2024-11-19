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
                .Include(x => x.Answers).ToListAsync();
        }

        public override void Delete(Question entity)
        {
            entity.IsDeleted = true;
        }

        public async Task<(IEnumerable<Question> questions, int pageCount)> SearchQuestions(
            string searchText,
            QuestionCategory? category,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {

            var query = _dbSet.AsQueryable().Where(q => !q.IsDeleted);

            if (!string.IsNullOrEmpty(searchText))
            {

                query = query.Where(q => EF.Functions.Like(q.Text.ToLower(), "%" + searchText.ToLower() + "%"));
            }


            if (category != null)
            {
                query = query.Where(q => q.Category == category);
            }


            var totalCount = await query.CountAsync(cancellationToken);
            var questions = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(q => q.Answers)
                .ToListAsync(cancellationToken);

            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            return (questions, pageCount);
        }


    }
}

