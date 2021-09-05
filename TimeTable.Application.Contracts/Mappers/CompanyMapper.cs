using TimeTable.Application.Contracts.Mappers.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Contracts.Mappers
{
    public class CompanyMapper : IMapper<Company, CompanyEntity>
    {
        public CompanyEntity Map(Company dto)
        {
            return new CompanyEntity()
            {
                Id = dto.Id,
                Name = dto.Name,
            };
        }

        public Company Map(CompanyEntity dto)
        {
            return new Company()
            {
                Id = dto.Id,
                Name = dto.Name,
            };
        }
    }
}
