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
    public class PersonService : BaseService<BasicReadingPerson, DetailedReadingPerson, CreationPerson, UpdatingBusinessPerson, PersonEntity>, IPersonService
    {
        private readonly ICompanyRepository companyRepository;

        public PersonService(IUnitOfWork unitOfWork, IPersonRepository repository, IAppConfig appConfig, ICompanyRepository companyRepository)
            : base(unitOfWork, repository, appConfig, new PersonMapper())
        {
            this.companyRepository = companyRepository;
        }

        protected override async Task ValidateEntityToAddAsync(CreationPerson businessModel)
        {
            await base.ValidateEntityToAddAsync(businessModel);
            bool existsPersonName = await repository.ExistsAsync(x => x.Name.ToLower() == businessModel.Name.ToLower());
            if (existsPersonName)
                throw new NotValidItemException(ErrorCodes.PERSON_NAME_EXISTS, $"The name {businessModel.Name} already exists in other person");

            bool existsCompany = await companyRepository.ExistsAsync(x => x.Id == businessModel.CompanyId);
            if (!existsCompany)
                throw new NotValidItemException(ErrorCodes.COMPANY_NOT_EXISTS, $"There is no company with the id {businessModel.Name}");
        }

        protected override async Task ValidateEntityToUpdateAsync(UpdatingBusinessPerson businessModel)
        {
            await base.ValidateEntityToUpdateAsync(businessModel);
            bool existsPersonName = await repository.ExistsAsync(x => x.Name.ToLower() == businessModel.Name.ToLower() && x.Id != businessModel.Id);
            if (existsPersonName)
                throw new NotValidItemException(ErrorCodes.PERSON_NAME_EXISTS, $"The name {businessModel.Name} already exists in other person");
        }
    }
}
