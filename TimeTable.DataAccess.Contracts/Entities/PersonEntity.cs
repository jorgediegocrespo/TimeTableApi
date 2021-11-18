using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class PersonEntity : IBaseWithIdEntity
    {
        public int Id { get; set; }
        public bool? IsAdmin { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int? CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }

        public List<TimeRecordEntity> TimeRecords { get; set; }
    }
}
