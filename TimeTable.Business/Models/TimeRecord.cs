using System;
using System.ComponentModel.DataAnnotations;
using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
{
    public class BasicReadingTimeRecord : IBasicReadingBusinessModel
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
    }

    public class DetailedReadingTimeRecord : BasicReadingTimeRecord, IDetailedReadingBusinessModel
    { }

    public class CreationTimeRecord : ICreationBusinessModel
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public DateTimeOffset StartDateTime { get; set; }

        public DateTimeOffset EndDateTime { get; set; }
    }

    public class UpdatingTimeRecord : CreationTimeRecord, IUpdatingBusinessModel
    {
        public int Id { get; set; }
    }
}
