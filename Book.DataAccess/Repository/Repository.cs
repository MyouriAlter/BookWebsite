using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Book.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        internal DbSet<T> DbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            this.DbSet = _dbContext.Set<T>();
        }

        public T Get(int id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> queryable = DbSet;

            if (filter != null)
                queryable = queryable.Where(filter);

            if (includeProperties != null)
            {
                queryable = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(queryable, (current, includeProperty) 
                        => current.Include(includeProperty));
            }

            return orderBy != null ? orderBy(queryable).ToList() : queryable.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> queryable = DbSet;

            if (filter != null)
                queryable = queryable.Where(filter);

            if (includeProperties != null)
            {
                queryable = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(queryable, (current, includeProperty)
                        => current.Include(includeProperty));
            }

            return queryable.FirstOrDefault();

        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Remove(int id)
        {
            T entity = DbSet.Find(id);
            Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            DbSet.RemoveRange(entity);
        }
    }
}
