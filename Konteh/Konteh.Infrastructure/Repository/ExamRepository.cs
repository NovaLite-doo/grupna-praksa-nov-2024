using Konteh.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konteh.Infrastructure.Repository
{
    public class ExamRepository : GenericRepository<Exam>
    {
        public ExamRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
