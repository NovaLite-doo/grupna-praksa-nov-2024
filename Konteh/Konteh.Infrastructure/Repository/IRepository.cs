using System.Linq.Expressions;

namespace Konteh.Infrastructure.Repository
{
    public interface IRepository<T>
    {
        public void Create(T entity);
        public void Delete(T entity);

        public Task<IList<T>> GetAll();
        public Task<T?> Get(int id);
        public Task SaveChanges();

        public Task<IList<T>> Search(Expression<Func<T, bool>> predicate);
    }
}
