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

        public CompanyEntity MapUpdating(Company businessModel)
        {
            return new CompanyEntity()
            {
                Id = businessModel.Id,
                Name = businessModel.Name,
            };
        }
    }
}
