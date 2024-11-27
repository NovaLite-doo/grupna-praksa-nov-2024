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

        public override async Task<IList<Exam>> Search(Expression<Func<Exam, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .Include(x => x.Questions)
                .Include(x => x.Candidate)
                .ToListAsync();
        } 
    }
}
