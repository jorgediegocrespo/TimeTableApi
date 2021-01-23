﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities.Base;
using TimeTable.DataAccess.Contracts.Repositories.Base;

namespace TimeTable.DataAccess.Repositories.Base
{
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class, IBaseWithIdEntity
    {
        protected readonly ITimeTableDbContext dbContext;

        public BaseRepository(ITimeTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected abstract DbSet<T> DbEntity { get; }

        public async Task<T> AddAsync(T entity)
        {
            await DbEntity.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entityToRemove = await DbEntity.SingleAsync(x => x.Id == id);
            DbEntity.Remove(entityToRemove);
            await dbContext.SaveChangesAsync();

            return;
        }

        public async Task<bool> ExistsAsync(int id) => await DbEntity.AnyAsync(x => x.Id == id);

        public virtual async Task<T> GetAsync(int id) => await DbEntity.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<T>> GetAllAsync() => await DbEntity.ToListAsync();

        public async Task<T> UpdateAsync(T entity)
        {
            DbEntity.Update(entity);
            await dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
