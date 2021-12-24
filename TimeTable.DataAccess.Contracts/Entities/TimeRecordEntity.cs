using System;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class TimeRecordEntity : IBaseWithIdEntity
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public virtual PersonEntity Person { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
    }
}
