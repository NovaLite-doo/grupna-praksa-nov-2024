using Konteh.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konteh.Infrastructure.Repository
{
    public class QuestionRepository : GenericRepository<Question>
    {
        public QuestionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IList<Question>> GetAll()
        {
            return await _dbSet
                .Where(q => !q.IsDeleted)
                .Include(x => x.Answers)
                .ToListAsync();
        }

        public override void Delete(Question entity)
        {
            entity.IsDeleted = true;
        }



    }
}

