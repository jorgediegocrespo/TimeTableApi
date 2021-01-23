using TimeTable.Application.Contracts.Models;
using TimeTable.Application.Mappers.Base;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Mappers
{
    public class BankDayMapper : IMapper<BankDay, BankDayEntity>
    {
        public BankDayEntity Map(BankDay businessModel)
        {
            return new BankDayEntity(); //TODO
        }

        public BankDay Map(BankDayEntity entity)
        {
            return new BankDay(); //TODO
        }
    }
}
