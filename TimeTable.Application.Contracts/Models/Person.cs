using System.Collections.Generic;
using TimeTable.Application.Contracts.Models.Base;

namespace TimeTable.Application.Contracts.Models
{
    public class Person : IBaseBusinessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public Company Company { get; set; }
        public List<TimeRecord> TimeRecords { get; set; }
    }
}
