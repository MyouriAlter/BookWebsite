using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Book.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        protected Repository(ApplicationDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> queryable = _dbSet;

            if (filter != null)
                queryable = queryable.Where(filter);

            if (includeProperties != null)
                queryable = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(queryable, (current, includeProperty)
                        => current.Include(includeProperty));

            return orderBy != null ? orderBy(queryable).ToList() : queryable.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> queryable = _dbSet;

            if (filter != null)
                queryable = queryable.Where(filter);

            if (includeProperties != null)
                queryable = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(queryable, (current, includeProperty)
                        => current.Include(includeProperty));

            return queryable.FirstOrDefault();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Remove(int id)
        {
            var entity = _dbSet.Find(id);
            Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
    }
}