using System.Net;

namespace PlatformRacing3.Server.API.Net
{
	public interface INetworkConnection : IDisposable
    {
        bool Disconnected { get; }

        uint SocketId { get; }
        IPAddress RemoteAddress { get; }

        TimeSpan LastRead { get; }

        event NetworkEvents.OnDisconnect OnDisconnect;

        void StartListening();
        void Send(byte[] data, int offset = 0, int? length = default);
        void Disconnect(string reason = default);
    }
}
