using Konteh.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Konteh.Infrastructure.Repository
{
    public class ExamRepository : GenericRepository<Exam>
    {
        public ExamRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IEnumerable<Exam>> GetAll() => await IncludeProperties().ToListAsync();

        public override async Task<IList<Exam>> Search(Expression<Func<Exam, bool>> predicate) => await IncludeProperties().Where(predicate).ToListAsync();

        private IQueryable<Exam> IncludeProperties()
        {
            var exams = _dbSet
                .Include(x => x.Candidate)
                .Include(x => x.Questions)
                    .ThenInclude(x => x.SubmittedAnswers)
                .Include(x => x.Questions)
                    .ThenInclude(x => x.Question)
                        .ThenInclude(x => x.Answers);

            return exams;
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
