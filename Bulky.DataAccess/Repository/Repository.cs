using ECommerceSite.DataAccess.Data;
using ECommerceSite.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerceSite.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool shouldTrack = false)
        {
            IQueryable<T> query;
            if (!shouldTrack)
            {
                query = dbSet.AsNoTracking();
            }
            else
            {
                query = dbSet;
            }
            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var inclueProperty in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(inclueProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }           

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var inclueProperty in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(inclueProperty);
                }
            }

            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
