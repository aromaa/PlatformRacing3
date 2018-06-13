using Platform_Racing_3_Server.Net;
using Platform_Racing_3_Server_API.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Handlers
{
    internal class SocketAwakeDataHandler : IDataHandler<INetworkConnectionGame>
    {
        private const string PRIVACY_POLICY_REQUEST = "<policy-file-request/>\0";
        private static readonly byte[] PrivacyPoliceResponse = Encoding.UTF8.GetBytes("<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\" /></cross-domain-policy>");

        internal static SocketAwakeDataHandler Instance { get; } = new SocketAwakeDataHandler();

        private SocketAwakeDataHandler()
        {

        }

        public void HandleData(INetworkConnectionGame connection, Span<byte> data)
        {
            string dataString = Encoding.UTF8.GetString(data.ToArray());
            if (SocketAwakeDataHandler.PRIVACY_POLICY_REQUEST == dataString)
            {
                connection.Send(SocketAwakeDataHandler.PrivacyPoliceResponse);
                connection.Disconnect("Privacy policy request");
            }
            else
            {
                connection.DataHandler = new PacketDataHandler(connection);
                connection.DataHandler.HandleData(connection, data); //Redirect
            }
        }
    }
}
