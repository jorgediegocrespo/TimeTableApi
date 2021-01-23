using TimeTable.Application.Contracts.Models;
using TimeTable.Application.Mappers.Base;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Mappers
{
    public class CompanyMapper : IMapper<Company, CompanyEntity>
    {
        public CompanyEntity Map(Company businessModel)
        {
            return new CompanyEntity(); //TODO
        }

        public Company Map(CompanyEntity entity)
        {
            return new Company(); //TODO
        }
    }
}
