using System.ComponentModel.DataAnnotations;

namespace TimeTable.Business.Models
{
    public class UserInfo
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
