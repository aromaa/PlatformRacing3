using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Platform_Racing_3_Server.Extensions
{
    public static class SocketErrorExtension
    {
        public static bool DisconnectFor(this SocketError socketError)
        {
            switch(socketError)
            {
                case SocketError.Success:
                case SocketError.IOPending:
                    return false;
                default:
                    return true;
            }
        }
    }
}
