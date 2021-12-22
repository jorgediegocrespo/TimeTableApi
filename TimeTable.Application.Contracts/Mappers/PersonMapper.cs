using TimeTable.Application.Contracts.Mappers.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Contracts.Mappers
{
    public class PersonMapper : IMapper<BasicReadingPerson, DetailedReadingPerson, CreationPerson, UpdatingBusinessPerson, PersonEntity>
    {
        public BasicReadingPerson MapBasicReading(PersonEntity entity)
        {
            return new BasicReadingPerson()
            {
                Id = entity.Id,
                Name = entity.Name,
                IsDefault = entity.IsDefault,
            };
        }

        public DetailedReadingPerson MapDetailedReading(PersonEntity entity)
        {
            return new DetailedReadingPerson()
            {
                Id = entity.Id,
                Name = entity.Name,
                IsDefault = entity.IsDefault,
            };
        }

        public PersonEntity MapCreating(CreationPerson businessModel)
        {
            return new PersonEntity()
            {
                Name = businessModel.Name,
                IsDefault = false
            };
        }        

        public void MapUpdating(PersonEntity entity, UpdatingBusinessPerson businessModel)
        {
            entity.Name = businessModel.Name;
            entity.IsDefault = false;
        }
    }
}
