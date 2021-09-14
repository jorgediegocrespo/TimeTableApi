using System;
using System.ComponentModel.DataAnnotations;
using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
{
    public class TimeRecord : IBaseBusinessModel
    {
        public int Id { get; set; }

        [Required]
        public int PersonId { get; set; }

        [Required]
        public DateTimeOffset StartDateTime { get; set; }

        public DateTimeOffset EndDateTime { get; set; }
    }
}
