using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using TimeTable.Business.ValidationAttributes;

namespace TimeTable.Business.Models
{
    public class ReadingPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string PictureUrl { get; set; }
        public byte[] RowVersion { get; set; }
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

        [ByteArrayMaxMegabytesValidation(500)]
        public byte[] Picture { get; set; }
    }

    public class UpdatingPerson
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        public byte[] RowVersion { get; set; }

        [ByteArrayMaxMegabytesValidation(500)]
        public byte[] Picture { get; set; }
    }
}
