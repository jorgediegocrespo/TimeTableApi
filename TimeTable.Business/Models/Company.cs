using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
{
    public class Company : IBaseBusinessModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        [MinLength(4)]
        public string Name { get; set; }

        public IEnumerable<Person> People { get; set; }
    }
}
