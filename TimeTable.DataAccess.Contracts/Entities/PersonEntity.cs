using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class PersonEntity : IBaseWithIdEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        public virtual List<TimeRecordEntity> TimeRecords { get; set; }
    }
}
