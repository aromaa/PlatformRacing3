namespace PlatformRacing3.Server.API.Net
{
	public interface INetworkManager : IDisposable
    {
        void AddListener(INetworkListener listener, bool start);
        void Shutdown();

        ICollection<INetworkConnection> GetActiveConnections();
    }
}
