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
        private readonly IPersonRepository personRepository;

        public CompanyService(ICompanyRepository repository, IAppConfig appConfig, IPersonRepository personRepository)
            : base(repository, appConfig, new CompanyMapper())
        {
            this.personRepository = personRepository;
        }

        protected override async Task ValidateEntityToAddAsync(Company entity)
        {
            await base.ValidateEntityToAddAsync(entity);
            bool existsCompanyName = await repository.ExistsAsync(x => x.Name.ToLower() == entity.Name.ToLower());
            if (existsCompanyName)
                throw new NotValidItemException(ErrorCodes.COMPANY_NAME_EXISTS, $"The name {entity.Name} already exists in other company");
        }

        protected override async Task ValidateEntityToUpdateAsync(Company entity)
        {
            await base.ValidateEntityToUpdateAsync(entity);
            bool existsCompanyName = await repository.ExistsAsync(x => x.Name.ToLower() == entity.Name.ToLower() && x.Id != entity.Id);
            if (existsCompanyName)
                throw new NotValidItemException(ErrorCodes.COMPANY_NAME_EXISTS, $"The name {entity.Name} already exists in other company");
        }

        protected override async Task ValidateEntityToDeleteAsync(int id)
        {
            await base.ValidateEntityToDeleteAsync(id);
            bool hasPeople = await personRepository.ExistsAsync(x => x.CompanyId == id);
            if (hasPeople)
                throw new NotValidItemException(ErrorCodes.COMPANY_NAME_EXISTS, $"The company has associated people");
        }
    }
}
