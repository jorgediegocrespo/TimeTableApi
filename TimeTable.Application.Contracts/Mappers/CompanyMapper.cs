using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Contracts.Mappers
{
    public class CompanyMapper
    {
        public BasicReadingCompany MapBasicReading(CompanyEntity entity)
        {
            return new BasicReadingCompany()
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}
