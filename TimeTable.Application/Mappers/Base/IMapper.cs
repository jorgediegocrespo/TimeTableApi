using TimeTable.Application.Contracts.Models.Base;

namespace TimeTable.Application.Mappers.Base
{
    public interface IMapper<T, R>
        where T : IBaseBusinessModel
        where R : class
    {
        T Map(R entity);
        R Map(T businessModel);
    }
}
