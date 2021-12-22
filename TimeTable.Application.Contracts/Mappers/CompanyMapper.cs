using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Contracts.Mappers
{
    public class CompanyMapper
    {
        public Company MapReading(CompanyEntity entity)
        {
            return new Company()
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }

        public void MapUpdating(CompanyEntity entity, Company businessModel)
        {
            entity.Name = businessModel.Name;
        }
    }
}
