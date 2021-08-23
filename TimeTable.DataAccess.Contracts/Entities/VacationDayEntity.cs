using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class VacationDayEntity : IBaseWithIdEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int VacationDays { get; set; }

        public int PersonId { get; set; }
        public PersonEntity Person { get; set; }
    }
}
