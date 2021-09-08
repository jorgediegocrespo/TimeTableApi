using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Mappers;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Exceptions;
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

        protected override async Task ValidateEntityToAddAsync(Company entity)
        {
            await base.ValidateEntityToAddAsync(entity);
            bool existsCompany = await repository.ExistsAsync(x => x.Name.ToLower() == entity.Name.ToLower());
            if (existsCompany)
                throw new NotValidItemException(ErrorCodes.COMPANY_NAME_EXISTS, $"The name {entity.Name} already exists in other company");
        }
    }
}
