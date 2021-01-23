using System;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class HolidayEntity : IBaseWithIdEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool Confirmed { get; set; }

        public PersonEntity PersonRequesting { get; set; }
        public PersonEntity ConfirmingPerson { get; set; }
    }
}
