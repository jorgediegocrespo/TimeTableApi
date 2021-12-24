using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class PersonRepository : BaseRepository<PersonEntity>, IPersonRepository
    {
        public PersonRepository(TimeTableDbContext dbContext) : base(dbContext)
        { }

        protected override DbSet<PersonEntity> DbEntity => dbContext.People;

        public async Task<bool> ExistsAsync(int id, string name) => 
            await DbEntity.AnyAsync(x => 
                x.Name.ToLowerInvariant() == name.ToLowerInvariant() &&
                x.Id != id);

        public async Task<IEnumerable<PersonEntity>> GetAllAsync() => await DbEntity.ToListAsync();

        public async Task<PersonEntity> GetAsync(int id) => await DbEntity.FirstOrDefaultAsync(x => x.Id == id);
        
        public async Task<PersonEntity> GetAsync(string userId) => await DbEntity.FirstOrDefaultAsync(x => x.UserId == userId);
        
        public async Task AddAsync(PersonEntity entity)
        {
            await DbEntity.AddAsync(entity);
        }

        public Task UpdateAsync(PersonEntity entity)
        {
            DbEntity.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entityToRemove = await DbEntity.SingleAsync(x => x.Id == id);
            DbEntity.Remove(entityToRemove);
        }
    }
}
