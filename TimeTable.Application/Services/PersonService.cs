using System.Collections.Generic;
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
    public class PersonService : BaseCrudService<BasicReadingPerson, DetailedReadingPerson, CreationPerson, UpdatingBusinessPerson, PersonEntity>, IPersonService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IUserService userService;
        
        public PersonService(IUnitOfWork unitOfWork, 
                             IPersonRepository repository, 
                             IAppConfig appConfig, 
                             ICompanyRepository companyRepository, 
                             IUserService userService)
            : base(unitOfWork, repository, appConfig, new PersonMapper())
        {
            this.companyRepository = companyRepository;
            this.userService = userService;
        }

        private IPersonRepository PersonRepository => (IPersonRepository)repository;

        public override Task<IEnumerable<BasicReadingPerson>> GetAllAsync()
        {
            throw new ForbidenActionException();
        }

        public override async Task<int> AddAsync(CreationPerson businessModel, bool withTransaction = true)
        {
            if (withTransaction)
                return await unitOfWork.SaveChangesInTransactionAsync(async () => await AddOperationsAsync(businessModel));
            else
                return await AddOperationsAsync(businessModel);
        }

        public override async Task DeleteAsync(int id, bool withTransaction)
        {
            if (withTransaction)
                await unitOfWork.ExecuteInTransactionAsync(async () => await DeleteOperationsAsync(id));
            else
                await DeleteOperationsAsync(id);
        }

        protected override async Task ValidateEntityToAddAsync(CreationPerson businessModel)
        {
            await base.ValidateEntityToAddAsync(businessModel);
            bool existsPersonName = await repository.ExistsAsync(x => x.Name.ToLower() == businessModel.Name.ToLower());
            if (existsPersonName)
                throw new NotValidItemException(ErrorCodes.PERSON_NAME_EXISTS, $"The name {businessModel.Name} already exists in other person");
        }

        protected override async Task ValidateEntityToUpdateAsync(UpdatingBusinessPerson businessModel)
        {
            await base.ValidateEntityToUpdateAsync(businessModel);
            bool existsPersonName = await repository.ExistsAsync(x => x.Name.ToLower() == businessModel.Name.ToLower() && x.Id != businessModel.Id);
            if (existsPersonName)
                throw new NotValidItemException(ErrorCodes.PERSON_NAME_EXISTS, $"The name {businessModel.Name} already exists in other person");

            int? contextPersonId = await userService.GetContextPersonIdAsync();
            if (!contextPersonId.HasValue || contextPersonId.Value != businessModel.Id)
                throw new ForbidenActionException();
        }

        protected override async Task ValidateEntityToDeleteAsync(int id)
        {
            await base.ValidateEntityToDeleteAsync(id);
            int? contextPersonId = await userService.GetContextPersonIdAsync();
            if (!contextPersonId.HasValue || contextPersonId.Value != id)
                throw new ForbidenActionException();
        }

        private async Task<int> AddOperationsAsync(CreationPerson businessModel)
        {
            await ValidateEntityToAddAsync(businessModel);
            string userId = await userService.RegisterAsync(new RegisterUserInfo
            {
                Email = businessModel.Email,
                UserName = businessModel.Name,
                Password = businessModel.Password,
            });
            

            var entity = await MapCreatingAsync(businessModel);
            entity.UserId = userId;
            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return entity.Id;
        }

        private async Task DeleteOperationsAsync(int id)
        {
            PersonEntity user = await repository.GetAsync(id);
            await base.DeleteAsync(id);            
            await userService.DeleteAsync(user.UserId);
        }
    }
}
