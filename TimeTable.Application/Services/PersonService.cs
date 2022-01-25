using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Exceptions;
using TimeTable.Application.Services.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IPersonRepository repository;
        private readonly IUserService userService;

        public PersonService(IUnitOfWork unitOfWork,
                             IPersonRepository repository,
                             IAppConfig appConfig,
                             IUserService userService)
            : base(unitOfWork, appConfig)
        {
            this.repository = repository;
            this.userService = userService;
        }

        public async Task<PaginatedResponse<ReadingPerson>> GetAllAsync(PaginationRequest request)
        {
            IEnumerable<PersonEntity> allEntities = await repository.GetAllAsync(request.PageSize, request.PageNumber);
            int count = await repository.GetTotalRecordsAsync();
            return new PaginatedResponse<ReadingPerson>
            {
                TotalRegisters = count,
                Result = allEntities.Select(x => MapReading(x))
            };
        }

        public async Task<ReadingPerson> GetAsync(int id)
        {
            PersonEntity entity = await repository.GetAsync(id);
            return MapReading(entity);
        }

        public async Task<ReadingPerson> GetOwnAsync()
        {
            int? id = await userService.GetContextPersonIdAsync();
            return await GetAsync(id.Value);
        }

        public async Task<int> AddAsync(CreatingPerson businessModel)
        {
            return await unitOfWork.SaveChangesInTransactionAsync(async () =>
            {
                await ValidateEntityToAddAsync(businessModel);
                string userId = await userService.RegisterAsync(new RegisterUserInfo
                {
                    Email = businessModel.Email,
                    UserName = businessModel.Name,
                    Password = businessModel.Password,
                });

                var entity = MapCreating(businessModel);
                entity.UserId = userId;
                await repository.AddAsync(entity);
                await unitOfWork.SaveChangesAsync();

                return entity.Id;
            });
        }

        public async Task UpdateAsync(UpdatingPerson businessModel)
        {
            PersonEntity entity = await repository.GetAsync(businessModel.Id);
            await ValidateEntityToUpdateAsync(entity, businessModel);
            MapUpdating(entity, businessModel);
            await repository.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                PersonEntity person = await repository.GetAsync(id);
                ValidateEntityToDelete(person, id);
                await repository.DeleteAsync(id);
                await unitOfWork.SaveChangesAsync();
                await userService.DeleteAsync(person.UserId);
            });
        }

        public async Task DeleteOwnAsync()
        {
            int? id = await userService.GetContextPersonIdAsync();
            await DeleteAsync(id.Value);
        }

        private ReadingPerson MapReading(PersonEntity entity)
        {
            return entity == null ? 
                null : 
                new ReadingPerson()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    IsDefault = entity.IsDefault,
                };
        }

        private PersonEntity MapCreating(CreatingPerson businessModel)
        {
            return new PersonEntity()
            {
                Name = businessModel.Name,
                IsDefault = false
            };
        }

        private void MapUpdating(PersonEntity entity, UpdatingPerson businessModel)
        {
            entity.Name = businessModel.Name;
            entity.IsDefault = false;
        }

        private async Task ValidateEntityToAddAsync(CreatingPerson businessModel)
        {
            bool existsPersonName = await repository.ExistsAsync(0, businessModel.Name);
            if (existsPersonName)
                throw new NotValidOperationException(ErrorCodes.PERSON_NAME_EXISTS, $"The name {businessModel.Name} already exists in other person");
        }

        private async Task ValidateEntityToUpdateAsync(PersonEntity entity, UpdatingPerson businessModel)
        {
            bool existsPersonName = await repository.ExistsAsync(businessModel.Id, businessModel.Name);
            if (existsPersonName)
                throw new NotValidOperationException(ErrorCodes.PERSON_NAME_EXISTS, $"The name {businessModel.Name} already exists in other person");

            int? contextPersonId = await userService.GetContextPersonIdAsync();
            if (!contextPersonId.HasValue || contextPersonId.Value != businessModel.Id)
                throw new ForbidenActionException();
        }

        private void ValidateEntityToDelete(PersonEntity person, int id)
        {
            if (person.IsDefault)
                throw new NotValidOperationException(ErrorCodes.PERSON_DEFAULT, $"A default person could not be removed");
        }
    }
}
