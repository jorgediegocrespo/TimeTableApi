﻿using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Business.Models;

namespace TimeTable.Application.Contracts.Services
{
    public interface ICompanyService : IBaseCrudService<BasicReadingCompany, DetailedReadingCompany, CreationCompany, UpdatingCompany>
    { }
}