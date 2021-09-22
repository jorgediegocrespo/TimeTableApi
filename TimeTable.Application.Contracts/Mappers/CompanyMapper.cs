using TimeTable.Application.Contracts.Mappers.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Contracts.Mappers
{
    public class CompanyMapper : IMapper<BasicReadingCompany, DetailedReadingCompany, CreationCompany, UpdatingCompany, CompanyEntity>
    {
        public BasicReadingCompany MapBasicReading(CompanyEntity entity)
        {
            return new BasicReadingCompany()
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
        public DetailedReadingCompany MapDetailedReading(CompanyEntity entity)
        {
            return new DetailedReadingCompany()
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }

        public CompanyEntity MapCreating(CreationCompany dto)
        {
            return new CompanyEntity()
            {
                Name = dto.Name,
            };
        }


        public CompanyEntity MapUpdating(UpdatingCompany dto)
        {
            return new CompanyEntity()
            {
                Id = dto.Id,
                Name = dto.Name,
            };
        }
    }
}
