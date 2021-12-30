using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class PersonEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        public virtual List<TimeRecordEntity> TimeRecords { get; set; }
    }
}
