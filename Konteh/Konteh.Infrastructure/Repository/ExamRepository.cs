using Konteh.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konteh.Infrastructure.Repository
{
    public class ExamRepository : GenericRepository<Exam>
    {
        public ExamRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Exam?> Get(int id)
        {
            return await _dbSet
                .Where(e => e.Id == id)
                .Include(e => e.Questions)
                .ThenInclude(eq => eq.Question)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync();
        }
    }
}
