using TimeTable.Business.Models.Base;

namespace TimeTable.Application.Contracts.Mappers.Base
{
    public interface IMapper<T, R>
        where T : IBaseBusinessModel
        where R : class
    {
        T Map(R dto);
        R Map(T dto);
    }
}
