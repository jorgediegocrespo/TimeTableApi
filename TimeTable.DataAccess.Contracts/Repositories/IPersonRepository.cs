﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories.Base;

namespace TimeTable.DataAccess.Contracts.Repositories
{
    public interface IPersonRepository : IBaseRepository<PersonEntity>
    {
        Task<bool> ExistsAsync(int id, string name);
        Task<IEnumerable<PersonEntity>> GetAllAsync();
        Task<PersonEntity> GetAsync(int id);
        Task<PersonEntity> GetAsync(string userId);
        Task AddAsync(PersonEntity entity);
        Task UpdateAsync(PersonEntity entity);
        Task DeleteAsync(int id);
    }
}
