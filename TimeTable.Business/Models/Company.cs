using System.ComponentModel.DataAnnotations;
using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
{
    public class BasicReadingCompany : IBasicReadingBusinessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DetailedReadingCompany : BasicReadingCompany, IDetailedReadingBusinessModel
    { }

    public class CreationCompany : ICreationBusinessModel
    {
        [Required]
        [MaxLength(450)]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        public CreationPerson Creator { get; set; }
    }

    public class UpdatingCompany : IUpdatingBusinessModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        [MinLength(4)]
        public string Name { get; set; }
    }
}
