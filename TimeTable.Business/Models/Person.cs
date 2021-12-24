using System.ComponentModel.DataAnnotations;
using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
{
    public class ReadingPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }

    public class CreatingPerson
    {
        [Required]
        [MaxLength(450)]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UpdatingPerson
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        [MinLength(4)]
        public string Name { get; set; }
    }
}
