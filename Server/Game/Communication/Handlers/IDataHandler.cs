using Platform_Racing_3_Server_API.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Handlers
{
    public interface IDataHandler<C> where C: INetworkConnection
    {
        void HandleData(C connection, Span<byte> data);
    }
}
