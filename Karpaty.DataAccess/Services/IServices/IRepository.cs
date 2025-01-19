using System.Linq.Expressions;

namespace Karpaty.Services.IServices
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Delete(T entity);
    }
}
