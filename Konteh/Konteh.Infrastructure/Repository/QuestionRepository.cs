using Konteh.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konteh.Infrastructure.Repository
{
    public class QuestionRepository : GenericRepository<Question>
    {
        public QuestionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IList<Question>> GetAll() => await _dbSet.Include(x => x.Answers).ToListAsync();
        public override async Task<Question?> Get(int id) => await _dbSet.Include(x => x.Answers).FirstOrDefaultAsync(q => q.Id == id);
    }
}
