using TimeTable.Application.Contracts.Models;
using TimeTable.Application.Mappers.Base;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Mappers
{
    public class PersonMapper : IMapper<Person, PersonEntity>
    {
        public PersonEntity Map(Person businessModel)
        {
            return new PersonEntity(); //TODO
        }

        public Person Map(PersonEntity entity)
        {
            return new Person(); //TODO
        }
    }
}
