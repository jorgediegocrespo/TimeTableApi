using TimeTable.Application.Contracts.Mappers.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Contracts.Mappers
{
    public class PersonMapper : IMapper<Person, PersonEntity>
    {
        public PersonEntity Map(Person dto)
        {
            return new PersonEntity()
            {
                Id = dto.Id,
                Name = dto.Name,
                IsAdmin = dto.IsAdmin,
            };
        }

        public Person Map(PersonEntity dto)
        {
            return new Person()
            {
                Id = dto.Id,
                Name = dto.Name,
                IsAdmin = dto.IsAdmin,
            };
        }
    }
}
