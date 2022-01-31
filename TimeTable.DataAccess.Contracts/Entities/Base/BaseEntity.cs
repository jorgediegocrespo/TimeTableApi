namespace TimeTable.DataAccess.Contracts.Entities.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
