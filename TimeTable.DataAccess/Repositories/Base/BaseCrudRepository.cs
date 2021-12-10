using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities.Base;
using TimeTable.DataAccess.Contracts.Repositories.Base;

namespace TimeTable.DataAccess.Repositories.Base
{
    public abstract class BaseCrudRepository<T> : BaseRepository<T>, ICrudRepository<T>
        where T : class, IBaseWithIdEntity
    {
        public BaseCrudRepository(TimeTableDbContext dbContext) : base(dbContext)
        { }

        public virtual async Task AddAsync(T entity)
        {
            await DbEntity.AddAsync(entity);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entityToRemove = await DbEntity.SingleAsync(x => x.Id == id);
            DbEntity.Remove(entityToRemove);
        }

        public virtual async Task<bool> ExistsAsync(int id) => await DbEntity.AnyAsync(x => x.Id == id);

        public virtual async Task<bool> ExistsAsync([NotNullAttribute] Expression<Func<T, bool>> expression) => await DbEntity.AnyAsync(expression);

        public virtual async Task<T> GetAsync(int id) => await DbEntity.FirstOrDefaultAsync(x => x.Id == id);

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await DbEntity.ToListAsync();

        public virtual Task UpdateAsync(T entity)
        {
            DbEntity.Attach(entity);
            DbEntity.Update(entity);
            return Task.CompletedTask;
        }
    }
}
