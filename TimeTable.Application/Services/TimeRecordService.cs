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
    public class TimeRecordService : BaseService, ITimeRecordService
    {
        private readonly ITimeRecordRepository repository;
        private readonly IUserService userService;

        public TimeRecordService(IUnitOfWork unitOfWork,
                                 ITimeRecordRepository repository,
                                 IAppConfig appConfig,
                                 IUserService userService)
            : base(unitOfWork, appConfig)
        {
            this.repository = repository;
            this.userService = userService;
        }

        public async Task<PaginatedResponse<ReadingTimeRecord>> GetAllAsync(PaginationRequest request)
        {
            IEnumerable<TimeRecordEntity> allEntities = await repository.GetAllAsync(request.PageSize, request.PageNumber);
            int count = await repository.GetTotalRecordsAsync();
            return new PaginatedResponse<ReadingTimeRecord>
            {
                TotalRegisters = count,
                Result = allEntities.Select(x => MapReading(x))
            };
        }

        public async Task<PaginatedResponse<ReadingTimeRecord>> GetAllOwnAsync(PaginationRequest request)
        {
            int? personId = await userService.GetContextPersonIdAsync();
            IEnumerable<TimeRecordEntity> allEntities = await repository.GetAllAsync(request.PageSize, request.PageNumber, personId.Value);
            int count = await repository.GetTotalRecordsAsync(personId.Value);
            return new PaginatedResponse<ReadingTimeRecord>
            {
                TotalRegisters = count,
                Result = allEntities.Select(x => MapReading(x))
            };
        }

        public async Task<ReadingTimeRecord> GetAsync(int id)
        {
            TimeRecordEntity entity = await repository.GetAsync(id);
            return MapReading(entity);
        }

        public async Task<ReadingTimeRecord> GetOwnAsync(int id)
        {
            int? personId = await userService.GetContextPersonIdAsync();
            TimeRecordEntity entity = await repository.GetAsync(id, personId.Value);
            return MapReading(entity);
        }

        public async Task<int> AddAsync(CreatingTimeRecord businessModel)
        {
            int? personId = await userService.GetContextPersonIdAsync();
            await ValidateEntityToAddAsync(businessModel);
            TimeRecordEntity entity = MapCreating(businessModel, personId.Value);
            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(UpdatingTimeRecord businessModel)
        {
            TimeRecordEntity entity = await repository.GetAsync(businessModel.Id);
            await ValidateEntityToUpdateAsync(entity, businessModel);
            MapUpdating(entity, businessModel);
            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            TimeRecordEntity entity = await repository.GetAsync(id);
            await ValidateEntityToDeleteAsync(entity);
            await repository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        private ReadingTimeRecord MapReading(TimeRecordEntity entity)
        {
            return new ReadingTimeRecord()
            {
                Id = entity.Id,
                PersonId = entity.PersonId,
                StartDateTime = entity.StartDateTime,
                EndDateTime = entity.EndDateTime,
            };
        }

        private TimeRecordEntity MapCreating(CreatingTimeRecord businessModel, int personId)
        {
            return new TimeRecordEntity()
            {
                PersonId = personId,
                StartDateTime = businessModel.StartDateTime,
                EndDateTime = businessModel.EndDateTime,
            };
        }

        private void MapUpdating(TimeRecordEntity entity, UpdatingTimeRecord businessModel)
        {
            entity.StartDateTime = businessModel.StartDateTime;
            entity.EndDateTime = businessModel.EndDateTime;
        }

        private async Task ValidateEntityToAddAsync(CreatingTimeRecord businessModel)
        {
            bool existsOverlappingTimeRecord = await repository.ExistsOverlappingAsync(0, businessModel.StartDateTime);
            if (existsOverlappingTimeRecord)
                throw new NotValidOperationException(ErrorCodes.TIME_RECORD_OVERLAPPING_EXISTS, $"There is another time record overlapping with this one");

            if (businessModel.EndDateTime == null)
                return;

            existsOverlappingTimeRecord = await repository.ExistsOverlappingAsync(0, businessModel.EndDateTime);
            if (existsOverlappingTimeRecord)
                throw new NotValidOperationException(ErrorCodes.TIME_RECORD_OVERLAPPING_EXISTS, $"There is another time record overlapping with this one");
        }

        private async Task ValidateEntityToUpdateAsync(TimeRecordEntity entity, UpdatingTimeRecord businessModel)
        {
            int? personId = await userService.GetContextPersonIdAsync();
            if (entity.PersonId != personId.Value)
                throw new ForbidenActionException();

            bool existsOverlappingTimeRecord = await repository.ExistsOverlappingAsync(businessModel.Id, businessModel.StartDateTime);
            if (existsOverlappingTimeRecord)
                throw new NotValidOperationException(ErrorCodes.TIME_RECORD_OVERLAPPING_EXISTS, $"There is another time record overlapping with this one");

            if (businessModel.EndDateTime == null)
                return;

            existsOverlappingTimeRecord = await repository.ExistsOverlappingAsync(businessModel.Id, businessModel.EndDateTime);
            if (existsOverlappingTimeRecord)
                throw new NotValidOperationException(ErrorCodes.TIME_RECORD_OVERLAPPING_EXISTS, $"There is another time record overlapping with this one");

        }

        private async Task ValidateEntityToDeleteAsync(TimeRecordEntity entity)
        {
            int? personId = await userService.GetContextPersonIdAsync();
            if (entity.PersonId != personId.Value)
                throw new ForbidenActionException();
        }
    }
}
