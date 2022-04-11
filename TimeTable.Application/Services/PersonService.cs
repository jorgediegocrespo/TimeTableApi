using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.Application.Constants;
using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Exceptions;
using TimeTable.Application.Services.Base;
using TimeTable.Business.ConstantValues;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private const string PERSON_PICTURE_CONTAINER = "People";
        private const string PERSON_PICTURE_EXTENSION = ".jpeg";
        private const string PERSON_PICTURE_CONTENT_TYPE = "image/jpeg";

        private readonly IPersonRepository repository;
        private readonly IUserService userService;
        private readonly IFileStorageService fileStorage;

        public PersonService(IUnitOfWork unitOfWork,
                             IPersonRepository repository,
                             IAppConfig appConfig,
                             IUserService userService,
                             IFileStorageService fileStorage)
            : base(unitOfWork, appConfig)
        {
            this.repository = repository;
            this.userService = userService;
            this.fileStorage = fileStorage;
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
                    Role = GetRoleString(businessModel.Role)
                });

                var entity = MapCreating(businessModel);
                entity.UserId = userId;
                entity.PictureUrl = await fileStorage.SaveFileAsync(businessModel.Picture, PERSON_PICTURE_EXTENSION, PERSON_PICTURE_CONTAINER, PERSON_PICTURE_CONTENT_TYPE);

                await repository.AddAsync(entity);
                await unitOfWork.SaveChangesAsync();

                return entity.Id;
            });
        }

        public async Task UpdateAsync(UpdatingPerson businessModel)
        {
            await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                PersonEntity entity = await repository.GetAsync(businessModel.Id);
                await ValidateEntityToUpdateAsync(entity, businessModel);

                PersonEntity entityToUpdate = await repository.AttachAsync(businessModel.Id, businessModel.RowVersion);
                MapUpdating(entityToUpdate, businessModel);
                entityToUpdate.PictureUrl = await fileStorage.UpdateFileAsync(businessModel.Picture, PERSON_PICTURE_EXTENSION, PERSON_PICTURE_CONTAINER, entity.PictureUrl, PERSON_PICTURE_CONTENT_TYPE);

                await unitOfWork.SaveChangesAsync();
            });
        }

        public async Task UpdateRoleAsync(UpdatingPersonRole businessModel)
        {
            await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                PersonEntity entty = await repository.GetAsync(businessModel.Id);
                ValidateEntityToUpdateRoleAsync(entty, businessModel);

                await userService.RemoveFromRoleAsync(entty.UserId);
                var newRole = GetRoleString(businessModel.Role);
                await userService.AddToRoleAsync(entty.UserId, newRole);
            });
        }

        public async Task DeleteAsync(DeleteRequest deleteRequest)
        {
            await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                PersonEntity person = await repository.GetAsync(deleteRequest.Id);
                ValidateEntityToDelete(person);

                await repository.DeleteAsync(deleteRequest.Id, deleteRequest.RowVersion);
                await unitOfWork.SaveChangesAsync();
                await userService.DeleteAsync(person.UserId);
                await fileStorage.DeleteFileAsync(PERSON_PICTURE_CONTAINER, person.PictureUrl);
            });
        }

        public async Task DeleteOwnAsync(DeleteOwnRequest deleteRequest)
        {
            int? id = await userService.GetContextPersonIdAsync();
            await DeleteAsync(new DeleteRequest { Id = id.Value, RowVersion = deleteRequest.RowVersion });
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
                    PictureUrl = entity.PictureUrl,
                    RowVersion = entity.RowVersion,
                };
        }

        private PersonEntity MapCreating(CreatingPerson businessModel)
        {
            return new PersonEntity()
            {
                Name = businessModel.Name,
                IsDefault = false,
            };
        }

        private void MapUpdating(PersonEntity entity, UpdatingPerson businessModel)
        {
            entity.Name = businessModel.Name;
            entity.IsDefault = false;
            entity.RowVersion = businessModel.RowVersion;
        }

        private async Task ValidateEntityToAddAsync(CreatingPerson businessModel)
        {
            bool existsPersonName = await repository.ExistsAsync(0, businessModel.Name);
            if (existsPersonName)
                throw new NotValidOperationException(ErrorCodes.PERSON_NAME_EXISTS, $"The name {businessModel.Name} already exists in other person");

            if (businessModel.Role == Role.None)
                throw new NotValidOperationException(ErrorCodes.INVALID_ROLE, "Invalid role");
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

        private void ValidateEntityToUpdateRoleAsync(PersonEntity entity, UpdatingPersonRole businessModel)
        {
            if (entity == null)
                throw new NotValidOperationException(ErrorCodes.ITEM_NOT_EXISTS, $"The person to update does not exits");

            if (businessModel.Role == Role.None)
                throw new NotValidOperationException(ErrorCodes.INVALID_ROLE, "Invalid role");

            if (!entity.RowVersion.SequenceEqual(businessModel.RowVersion))
                throw new DbUpdateConcurrencyException();
        }

        private void ValidateEntityToDelete(PersonEntity person)
        {
            if (person == null)
                throw new NotValidOperationException(ErrorCodes.ITEM_NOT_EXISTS, $"The person to remove does not exits");

            if (person.IsDefault)
                throw new NotValidOperationException(ErrorCodes.PERSON_DEFAULT, $"A default person could not be removed");
        }

        private string GetRoleString(Role role)
        {
            switch (role)
            {
                case Role.Admin:
                    return RolesConsts.ADMIN;
                case Role.Employee:
                    return RolesConsts.EMPLOYEE;
                default:
                    return string.Empty;
            }
        }
    }
}
