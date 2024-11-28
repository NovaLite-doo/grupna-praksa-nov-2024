using System.Linq.Expressions;

namespace Konteh.Infrastructure.Repository
{
    public interface IRepository<T>
    {
        public void Create(T entity);
        public void Delete(T entity);

        public Task<IEnumerable<T>> GetAll();
        public Task<T?> Get(int id);
        public Task SaveChanges();

        public Task<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate);
    }
}
