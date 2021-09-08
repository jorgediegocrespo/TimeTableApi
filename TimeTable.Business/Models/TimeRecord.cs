using System;
using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
{
    public class TimeRecord : IBaseBusinessModel
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
    }
}
