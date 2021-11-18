﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class PersonRepository : BaseRepository<PersonEntity>, IPersonRepository
    {
        protected override DbSet<PersonEntity> DbEntity => dbContext.People;

        public PersonRepository(TimeTableDbContext dbContext) : base(dbContext)
        { }

        public async Task<PersonEntity> GetByUserIdAsync(string userId) => await DbEntity.FirstOrDefaultAsync(x => x.UserId == userId);
        public async Task<IEnumerable<PersonEntity>> GetAllByCompanyIdAsync(int companyId) => await DbEntity.Where(x => x.CompanyId == companyId).ToListAsync();
    }
}
