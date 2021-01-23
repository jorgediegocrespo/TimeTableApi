namespace TimeTable.Application.Contracts.Configuration
{
    public interface IAppConfig
    {
        int MaxTrys { get; }
        int SecondToWait { get; }
    }
}
