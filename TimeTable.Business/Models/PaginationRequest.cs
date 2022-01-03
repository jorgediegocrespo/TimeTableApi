using System.Collections.Generic;
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

    public class PaginatedResponse<T> where T : class
    {
        public int TotalRegisters { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
