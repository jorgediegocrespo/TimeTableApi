﻿using System.Threading.Tasks;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Business.Models;

namespace TimeTable.Application.Contracts.Services
{
    public interface IPersonService : IBaseCrudService<BasicReadingPerson, DetailedReadingPerson, CreationPerson, UpdatingBusinessPerson>
    {
        Task<DetailedReadingPerson> GetOwnAsync();
        Task DeleteOwnAsync();
    }
}
