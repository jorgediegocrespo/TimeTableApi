using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Mappers;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Services.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Services
{
    public class CompanyService : BaseCrudService<BasicReadingCompany, DetailedReadingCompany, CreationCompany, UpdatingCompany, CompanyEntity>, ICompanyService
    {
        public CompanyService(IUnitOfWork unitOfWork, 
                              ICompanyRepository repository, 
                              IAppConfig appConfig)
            : base(unitOfWork, repository, appConfig, new CompanyMapper())
        { }
    }
}
