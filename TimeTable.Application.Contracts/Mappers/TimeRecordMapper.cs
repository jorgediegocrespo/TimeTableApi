using TimeTable.Application.Contracts.Mappers.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Contracts.Mappers
{
    public class TimeRecordMapper : IMapper<TimeRecord, TimeRecordEntity>
    {
        public TimeRecordEntity Map(TimeRecord dto)
        {
            return new TimeRecordEntity()
            {
                Id = dto.Id,
                PersonId = dto.PersonId,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
            };
        }

        public TimeRecord Map(TimeRecordEntity dto)
        {
            return new TimeRecord()
            {
                Id = dto.Id,
                PersonId = dto.PersonId,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
            };
        }
    }
}
