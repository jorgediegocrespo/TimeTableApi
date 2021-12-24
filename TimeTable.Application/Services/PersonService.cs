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
        private readonly IUserService userService;
        
        public PersonService(IUnitOfWork unitOfWork, 
                             IPersonRepository repository, 
                             IAppConfig appConfig, 
                             IUserService userService)
            : base(unitOfWork, repository, appConfig, new PersonMapper())
        {
            this.userService = userService;
        }

        public virtual async Task<DetailedReadingPerson> GetOwnAsync()
        {
            int? id = await userService.GetContextPersonIdAsync();
            return await GetAsync(id.Value);
        }

        public override async Task<int> AddAsync(CreationPerson businessModel, bool withTransaction = true)
        {
            if (withTransaction)
                return await unitOfWork.SaveChangesInTransactionAsync(async () => await AddOperationsAsync(businessModel));
            else
                return await AddOperationsAsync(businessModel);
        }

        public async Task DeleteOwnAsync()
        {
            int? id = await userService.GetContextPersonIdAsync();
            await DeleteAsync(id.Value);
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
                throw new NotValidOperationException(ErrorCodes.PERSON_NAME_EXISTS, $"The name {businessModel.Name} already exists in other person");
        }

        protected override async Task ValidateEntityToUpdateAsync(PersonEntity entity, UpdatingBusinessPerson businessModel)
        {
            await base.ValidateEntityToUpdateAsync(entity, businessModel);
            bool existsPersonName = await repository.ExistsAsync(x => x.Name.ToLower() == businessModel.Name.ToLower() && x.Id != businessModel.Id);
            if (existsPersonName)
                throw new NotValidOperationException(ErrorCodes.PERSON_NAME_EXISTS, $"The name {businessModel.Name} already exists in other person");

            int? contextPersonId = await userService.GetContextPersonIdAsync();
            if (!contextPersonId.HasValue || contextPersonId.Value != businessModel.Id)
                throw new ForbidenActionException();
        }

        protected override async Task ValidateEntityToDeleteAsync(int id)
        {
            await base.ValidateEntityToDeleteAsync(id);
            PersonEntity person = await repository.GetAsync(id);
            if (person.IsDefault)
                throw new NotValidOperationException(ErrorCodes.PERSON_DEFAULT, $"A default person could not be removed");
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
            PersonEntity person = await repository.GetAsync(id);
            await base.DeleteAsync(id, false);            
            await userService.DeleteAsync(person.UserId);
        }
    }
}
