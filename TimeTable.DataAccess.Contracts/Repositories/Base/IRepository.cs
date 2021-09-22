﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Repositories.Base
{
    public interface IRepository<T> where T : IBaseWithIdEntity
    {
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync([NotNullAttribute] Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task DeleteAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<T> AddAsync(T entity);
    }
}