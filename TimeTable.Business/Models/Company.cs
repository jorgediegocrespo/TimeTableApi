using System.Collections.Generic;
using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
{
    public class Company : IBaseBusinessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Person> People { get; set; }
    }
}
