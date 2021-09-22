namespace TimeTable.Business.Models.Base
{
    public interface IBaseBusinessModel
    {
        int Id { get; }
    }

    public interface IBasicReadingBusinessModel
    {
        int Id { get; }
    }

    public interface IDetailedReadingBusinessModel
    {
        int Id { get; }
    }

    public interface ICreationBusinessModel
    { }

    public interface IUpdatingBusinessModel
    {
        int Id { get; }
    }
}
