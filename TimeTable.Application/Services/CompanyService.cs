using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Mappers;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Services.Base;
using TimeTable.Business.Models;
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
