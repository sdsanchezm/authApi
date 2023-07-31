using AuthApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Domain.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly AuthApiContext _authApiContext;
        internal DbSet<T> _dbSet;

        public Repository(AuthApiContext authApiContext)
        {
            _authApiContext = authApiContext;
            _dbSet = _authApiContext.Set<T>();
        }

        public virtual Task<List<T>> GetAsync()
        {
            return _dbSet.ToListAsync();
        }

        public virtual Task<List<T>> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToListAsync();
            }
            else
            {
                return query.ToListAsync();
            }
        }

        public virtual Task<T> GetSingle(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefaultAsync();
            }
            else
            {
                return query.FirstOrDefaultAsync();
            }
        }

        public virtual async Task<T> GetByID(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            _dbSet.AddAsync(entity);
            await _authApiContext.SaveChangesAsync();

            return entity;
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_authApiContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _authApiContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
