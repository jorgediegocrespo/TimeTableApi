﻿using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories.Base;

namespace TimeTable.DataAccess.Contracts.Repositories
{
    public interface IPersonRepository : ICrudRepository<PersonEntity>
    {
        Task<PersonEntity> GetByUserIdAsync(string userId);
    }
}
