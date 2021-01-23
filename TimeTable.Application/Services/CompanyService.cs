using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Models;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Mappers;
using TimeTable.Application.Services.Base;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Services
{
    public class CompanyService : BaseService<Company, CompanyEntity>, ICompanyService
    {
        public CompanyService(ICompanyRepository repository, IAppConfig appConfig)
            : base(repository, appConfig, new CompanyMapper())
        { }
    }
}
