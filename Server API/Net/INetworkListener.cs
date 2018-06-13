using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Platform_Racing_3_Server_API.Net
{
    public interface INetworkListener : IDisposable
    {
        IPEndPoint Bind { get; }
        int Backlog { get; }

        void StartListening();
        void StopListening();
    }
}
