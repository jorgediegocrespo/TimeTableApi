using System.Collections.Generic;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Entities
{
    public class CompanyEntity : IBaseWithIdEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
