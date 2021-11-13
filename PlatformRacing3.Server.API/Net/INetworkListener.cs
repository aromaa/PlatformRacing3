using System.Net;

namespace PlatformRacing3.Server.API.Net
{
	public interface INetworkListener : IDisposable
    {
        IPEndPoint Bind { get; }
        int Backlog { get; }

        void StartListening();
        void StopListening();
    }
}
