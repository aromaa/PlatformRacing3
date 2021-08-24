namespace PlatformRacing3.Common.Config
{
    public interface IRedisConfig
    {
        string RedisHost { get; }
        uint RedisPort { get; }
    }
}
