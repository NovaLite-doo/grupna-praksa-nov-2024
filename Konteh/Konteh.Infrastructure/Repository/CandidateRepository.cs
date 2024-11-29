using Konteh.Domain;

namespace Konteh.Infrastructure.Repository
{
    public class CandidateRepository : GenericRepository<Candidate>
    {
        public CandidateRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
