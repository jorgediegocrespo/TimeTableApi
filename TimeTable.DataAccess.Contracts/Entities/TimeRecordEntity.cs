using System;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class TimeRecordEntity
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public virtual PersonEntity Person { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
    }
}
