using System.Collections.Generic;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class CompanyEntity : IBaseWithIdEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<PersonEntity> People { get; set; }
        public List<BankDayEntity> BankDays { get; set; }
    }
}
