namespace TimeTable.DataAccess.Contracts.Entities.Base
{
    public interface IBaseWithIdEntity : IBaseEntity
    {
        int Id { get; set; }
    }
}
