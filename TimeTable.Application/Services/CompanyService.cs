using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Mappers;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Exceptions;
using TimeTable.Application.Services.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Services
{
    public class CompanyService : BaseCrudService<BasicReadingCompany, DetailedReadingCompany, CreationCompany, UpdatingCompany, CompanyEntity>, ICompanyService
    {
        private readonly IPersonRepository personRepository;

        public CompanyService(IUnitOfWork unitOfWork, ICompanyRepository repository, IAppConfig appConfig, IPersonService personService, IPersonRepository personRepository)
            : base(unitOfWork, repository, appConfig, new CompanyMapper())
        {
            this.personRepository = personRepository;
        }

        protected override async Task ValidateEntityToAddAsync(CreationCompany businessModel)
        {
            await base.ValidateEntityToAddAsync(businessModel);
            bool existsCompanyName = await repository.ExistsAsync(x => x.Name.ToLower() == businessModel.Name.ToLower());
            if (existsCompanyName)
                throw new NotValidItemException(ErrorCodes.COMPANY_NAME_EXISTS, $"The name {businessModel.Name} already exists in other company");

            bool existsPerson = await personRepository.ExistsAsync(x => x.Id == businessModel.CreatorId);
            if (!existsPerson)
                throw new NotValidItemException(ErrorCodes.COMPANY_NEED_CREATOR, $"There is not any person with the id {businessModel.CreatorId}");
        }

        protected override async Task ValidateEntityToUpdateAsync(UpdatingCompany businessModel)
        {
            await base.ValidateEntityToUpdateAsync(businessModel);
            bool existsCompanyName = await repository.ExistsAsync(x => x.Name.ToLower() == businessModel.Name.ToLower() && x.Id != businessModel.Id);
            if (existsCompanyName)
                throw new NotValidItemException(ErrorCodes.COMPANY_NAME_EXISTS, $"The name {businessModel.Name} already exists in other company");
        }

        protected override async Task ValidateEntityToDeleteAsync(int id)
        {
            await base.ValidateEntityToDeleteAsync(id);
            bool hasPeople = await personRepository.ExistsAsync(x => x.CompanyId == id);
            if (hasPeople)
                throw new NotValidItemException(ErrorCodes.COMPANY_HAS_PEOPLE, $"The company has associated people");
        }
    }
}
