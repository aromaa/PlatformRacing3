using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server_API.Net
{
    public interface INetworkManager : IDisposable
    {
        void AddListener(INetworkListener listener, bool start);
        void Shutdown();

        ICollection<INetworkConnection> GetActiveConnections();
    }
}
