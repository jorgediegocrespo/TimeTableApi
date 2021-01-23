using System.Collections.Generic;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class PersonEntity : IBaseWithIdEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }

        public CompanyEntity Company { get; set; }
        public List<VacationDayEntity> VacationDays { get; set; }
        public List<HolidayEntity> HolidaysRequested { get; set; }
        public List<HolidayEntity> ConfirmedHolidays { get; set; }
    }
}
