using System.ComponentModel.DataAnnotations;

namespace TimeTable.Business.Models
{
    public class PaginationRequest
    {
        [Required]
        [Range(1, 100)]
        public int PageSize { get; set; }

        [Required]
        public int PageNumber { get; set; }
    }
}
