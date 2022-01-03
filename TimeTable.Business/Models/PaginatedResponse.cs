using System.Collections.Generic;

namespace TimeTable.Business.Models
{
    public class PaginatedResponse<T> where T : class
    {
        public int TotalRegisters { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
