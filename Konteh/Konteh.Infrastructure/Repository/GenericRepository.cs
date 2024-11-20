using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Konteh.Infrastructure.Repository
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public void Create(T entity) => _dbSet.Add(entity);

        public virtual void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<T?> Get(int id) => await _dbSet.FindAsync(id);

        public virtual async Task<IEnumerable<T>> GetAll() => await _dbSet.ToListAsync();

        public async Task SaveChanges() => await _dbContext.SaveChangesAsync();

        public async Task<IList<T>> Search(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
    }
}
