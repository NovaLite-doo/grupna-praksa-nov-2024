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

        public override async Task<Exam?> Get(int id) => await IncludeProperties().FirstOrDefaultAsync(e => e.Id == id);
        public override async Task<IEnumerable<Exam>> GetAll() => await IncludeProperties().OrderByDescending(x => x.DateTimeStarted).ToListAsync();

        public override async Task<IList<Exam>> Search(Expression<Func<Exam, bool>> predicate) => await IncludeProperties().Where(predicate).OrderByDescending(x => x.DateTimeStarted).ToListAsync();

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
    }
}
