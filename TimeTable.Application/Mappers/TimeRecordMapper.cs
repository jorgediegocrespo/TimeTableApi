using TimeTable.Application.Contracts.Models;
using TimeTable.Application.Mappers.Base;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Mappers
{
    public class TimeRecordMapper : IMapper<TimeRecord, TimeRecordEntity>
    {
        public TimeRecordEntity Map(TimeRecord businessModel)
        {
            return new TimeRecordEntity()
            {
                Id = businessModel.Id,
                StartDateTime = businessModel.StartDateTime,
                EndDateTime = businessModel.EndDateTime,
            };
        }

        public TimeRecord Map(TimeRecordEntity entity)
        {
            return new TimeRecord(); //TODO
        }
    }
}
