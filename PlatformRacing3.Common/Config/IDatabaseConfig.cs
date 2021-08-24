namespace PlatformRacing3.Common.Config
{
    public interface IDatabaseConfig
    {
        string DatabaseHost { get; }
        uint DatabasePort { get; }

        string DatabaseUser { get; }
        string DatabasePass { get; }
        string DatabaseName { get; }
    }
}
