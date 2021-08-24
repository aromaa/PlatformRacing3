using Platform_Racing_3_Server_API.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server_API.Net
{
    public class NetworkEvents
    {
        public delegate void OnDisconnect(INetworkConnection networkConnection);
    }
}
