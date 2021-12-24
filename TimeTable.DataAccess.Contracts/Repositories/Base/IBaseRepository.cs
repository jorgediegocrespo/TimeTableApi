using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Repositories.Base
{
    public interface IBaseRepository<T> where T : IBaseEntity
    { }
}
