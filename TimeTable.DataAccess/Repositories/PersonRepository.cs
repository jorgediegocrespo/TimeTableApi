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

        public async Task<IEnumerable<PersonEntity>> GetAllAsync(int pageSize, int pageNumber)
        {
            var result = await DbEntity.OrderBy(x => x.Name).Paginate(pageSize, pageNumber).ToListAsync();
            dbContext.ChangeTracker.Clear();

            return result;
        }

        public async Task<PersonEntity> GetAsync(int id)
        {
            var result = await DbEntity.FirstOrDefaultAsync(x => x.Id == id);
            dbContext.ChangeTracker.Clear();

            return result;
        }

        public async Task<PersonEntity> GetAsync(string userId)
        {
            var result = await DbEntity.FirstOrDefaultAsync(x => x.UserId == userId);
            dbContext.ChangeTracker.Clear();

            return result;
        }
        
        public async Task AddAsync(PersonEntity entity)
        {
            await DbEntity.AddAsync(entity);
        }

        public Task<PersonEntity> AttachAsync(int id, byte[] rowVersion)
        {
            PersonEntity entity = new PersonEntity { Id = id, RowVersion = rowVersion };
            DbEntity.Attach(entity);

            return Task.FromResult(entity);
        }

        public Task DeleteAsync(int id, byte[] rowVersion)
        {
            PersonEntity entity = new PersonEntity { Id = id, RowVersion = rowVersion };
            DbEntity.Attach(entity);

            DbEntity.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
