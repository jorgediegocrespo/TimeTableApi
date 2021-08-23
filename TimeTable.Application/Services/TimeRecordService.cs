using TimeTable.Application.Contracts.Configuration;
using TimeTable.Application.Contracts.Models;
using TimeTable.Application.Contracts.Services;
using TimeTable.Application.Mappers;
using TimeTable.Application.Services.Base;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;

namespace TimeTable.Application.Services
{
    public class TimeRecordService : BaseService<TimeRecord, TimeRecordEntity>, ITimeRecordService
    {
        public TimeRecordService(ITimeRecordRepository repository, IAppConfig appConfig)
            : base(repository, appConfig, new TimeRecordMapper())
        { }
    }
}
