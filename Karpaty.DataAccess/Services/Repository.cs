using Karpaty.Data;
using Karpaty.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Karpaty.Services
{
    public class Repository<T> : IRepository<T>  where T : class
    {
        private readonly ApplicationDbContext _db;
        public DbSet<T> dbset;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbset = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            
            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) { 
                    query=query.Include(property);
                }
            }

            return query.FirstOrDefault()!;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.ToList();
        }
    }
}
