using System.Collections.Generic;
using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
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
