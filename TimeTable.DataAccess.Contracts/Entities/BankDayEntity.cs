using System;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class BankDayEntity : IBaseWithIdEntity
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }

        public CompanyEntity Company { get; set; }
    }
}
