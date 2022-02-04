using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Extensions;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class PersonRepository : BaseRepository<PersonEntity>, IPersonRepository
    {
        public PersonRepository(TimeTableDbContext dbContext) : base(dbContext)
        { }

        protected override DbSet<PersonEntity> DbEntity => dbContext.People;

        public async Task<bool> ExistsAsync(int id, string name) => 
            await DbEntity.Where(x => 
                x.Name.ToLower().Trim() == name.ToLower().Trim() &&
                x.Id != id)
            .AnyAsync();

        public async Task<int> GetTotalRecordsAsync() => await DbEntity.CountAsync();

        public async Task<IEnumerable<PersonEntity>> GetAllAsync(int pageSize, int pageNumber) => 
            await DbEntity.OrderBy(x => x.Name).Paginate(pageSize, pageNumber).ToListAsync();

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
