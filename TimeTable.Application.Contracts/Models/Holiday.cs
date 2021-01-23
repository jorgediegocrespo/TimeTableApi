using System;
using TimeTable.Application.Contracts.Models.Base;

namespace TimeTable.Application.Contracts.Models
{
    public class Holiday : IBaseBusinessModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool Confirmed { get; set; }
        public Person PersonRequesting { get; set; }
        public Person ConfirmingPerson { get; set; }
    }
}
