using System.ComponentModel.DataAnnotations;
using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
{
    public class BasicReadingPerson : IBasicReadingBusinessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsAdmin { get; set; }
    }

    public class DetailedReadingPerson : BasicReadingPerson, IDetailedReadingBusinessModel
    { }

    public class CreationPerson : ICreationBusinessModel
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

    public class UpdatingBusinessPerson : IUpdatingBusinessModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        [MinLength(4)]
        public string Name { get; set; }
    }
}
