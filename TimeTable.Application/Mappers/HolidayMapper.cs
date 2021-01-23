using TimeTable.Application.Contracts.Models;
using TimeTable.Application.Mappers.Base;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Mappers
{
    public class HolidayMapper : IMapper<Holiday, HolidayEntity>
    {
        public HolidayEntity Map(Holiday businessModel)
        {
            return new HolidayEntity(); //TODO
        }

        public Holiday Map(HolidayEntity entity)
        {
            return new Holiday(); //TODO
        }
    }
}
