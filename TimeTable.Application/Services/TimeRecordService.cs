using System;
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
    public class TimeRecordService : BaseCrudService<BasicReadingTimeRecord, DetailedReadingTimeRecord, CreationTimeRecord, UpdatingTimeRecord, TimeRecordEntity>, ITimeRecordService
    {
        public TimeRecordService(IUnitOfWork unitOfWork, ITimeRecordRepository repository, IAppConfig appConfig)
            : base(unitOfWork, repository, appConfig, new TimeRecordMapper())
        { }

        protected override async Task ValidateEntityToAddAsync(CreationTimeRecord businessModel)
        {
            await base.ValidateEntityToAddAsync(businessModel);
            await ValidateOverlappingTimeRecord(businessModel.StartDateTime);
            await ValidateOverlappingTimeRecord(businessModel.EndDateTime);
        }

        protected override async Task ValidateEntityToUpdateAsync(UpdatingTimeRecord businessModel)
        {
            await base.ValidateEntityToUpdateAsync(businessModel);
            await ValidateOverlappingTimeRecord(businessModel.StartDateTime, businessModel.Id);
            await ValidateOverlappingTimeRecord(businessModel.EndDateTime, businessModel.Id);
        }

        private async Task ValidateOverlappingTimeRecord(DateTimeOffset dateTime)
        {
            if (dateTime == null)
                return;

            bool existsOverlappingTimeRecord = await repository.ExistsAsync(x =>
                x.StartDateTime.UtcDateTime <= dateTime.UtcDateTime &&
                x.EndDateTime != null &&
                x.EndDateTime.UtcDateTime >= dateTime.UtcDateTime);
            if (existsOverlappingTimeRecord)
                throw new NotValidOperationException(ErrorCodes.TIME_RECORD_OVERLAPPING_EXISTS, $"There is another time record overlapping with this one");
        }

        private async Task ValidateOverlappingTimeRecord(DateTimeOffset dateTime, int id)
        {
            if (dateTime == null)
                return;

            bool existsOverlappingTimeRecord = await repository.ExistsAsync(x =>
                x.StartDateTime.UtcDateTime <= dateTime.UtcDateTime &&
                x.EndDateTime != null &&
                x.EndDateTime.UtcDateTime >= dateTime.UtcDateTime &&
                x.Id != id);
            if (existsOverlappingTimeRecord)
                throw new NotValidOperationException(ErrorCodes.TIME_RECORD_OVERLAPPING_EXISTS, $"There is another time record overlapping with this one");
        }
    }
}
