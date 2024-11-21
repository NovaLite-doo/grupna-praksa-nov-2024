using Konteh.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Konteh.Infrastructure.Repository
{
    public class ExamRepository : GenericRepository<Exam>
    {
        public ExamRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public override async Task<IList<Exam>> Search(Expression<Func<Exam, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .Include(e => e.Candidate)
                .Include(e => e.Questions)
                .ToListAsync();
        }

        public override async Task<Exam?> Get(int id)
        {
            return await _dbSet
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

    }
}

