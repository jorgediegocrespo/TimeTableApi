using TimeTable.Business.Models.Base;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.Application.Contracts.Mappers.Base
{
    public interface IMapper<BR, DR, C, U, E>
        where BR : IBasicReadingBusinessModel
        where DR : IDetailedReadingBusinessModel
        where C : ICreationBusinessModel
        where U : IUpdatingBusinessModel
        where E : IBaseEntity
    {
        BR MapBasicReading(E entity);
        DR MapDetailedReading(E entity);
        E MapCreating(C businessModel);
        void MapUpdating(E entity, U businessModel);
    }
}
