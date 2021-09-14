using System.ComponentModel.DataAnnotations;
using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
{
    public class Person : IBaseBusinessModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public int CompanyId { get; set; }
    }
}
