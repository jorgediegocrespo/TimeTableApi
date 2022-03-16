using System.ComponentModel.DataAnnotations;

namespace TimeTable.Business.Models
{
    public class Company
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(450)]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        public byte[] RowVersion { get; set; }
    }
}
