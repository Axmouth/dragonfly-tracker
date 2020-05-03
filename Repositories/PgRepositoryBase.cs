using DragonflyTracker.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonflyTracker.Repositories
{
    public abstract class PgRepositoryBase<T>: IRepositoryBase<T> where T : class
    {
        protected PgMainDataContext _dataContext { get; set; }

        public PgRepositoryBase(PgMainDataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public IQueryable<T> FindAll()
        {
            return this._dataContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this._dataContext.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task CreateAsync(T entity)
        {
            await this._dataContext.Set<T>().AddAsync(entity).ConfigureAwait(false);
        }

        public void Update(T entity)
        {
            this._dataContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this._dataContext.Set<T>().Attach(entity);
            this._dataContext.Set<T>().Remove(entity);
        }
        public async Task<int> SaveAsync()
        {
            return await _dataContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dataContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
