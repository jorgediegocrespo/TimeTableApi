using TimeTable.Application.Contracts.Mappers.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Contracts.Mappers
{
    public class PersonMapper : IMapper<BasicReadingPerson, DetailedReadingPerson, CreationBusinessPerson, UpdatingBusinessPerson, PersonEntity>
    {
        public BasicReadingPerson MapBasicReading(PersonEntity entity)
        {
            return new BasicReadingPerson()
            {
                Id = entity.Id,
                Name = entity.Name,
                IsAdmin = entity.IsAdmin,
                CompanyId = entity.CompanyId
            };
        }

        public DetailedReadingPerson MapDetailedReading(PersonEntity entity)
        {
            return new DetailedReadingPerson()
            {
                Id = entity.Id,
                Name = entity.Name,
                IsAdmin = entity.IsAdmin,
                CompanyId = entity.CompanyId
            };
        }

        public PersonEntity MapCreating(CreationBusinessPerson businessModel)
        {
            return new PersonEntity()
            {
                Name = businessModel.Name,
                IsAdmin = businessModel.IsAdmin,
                CompanyId = businessModel.CompanyId
            };
        }        

        public PersonEntity MapUpdating(UpdatingBusinessPerson businessModel)
        {
            return new PersonEntity()
            {
                Id = businessModel.Id,
                Name = businessModel.Name,
                IsAdmin = businessModel.IsAdmin
            };
        }
    }
}
