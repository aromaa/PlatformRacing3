using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json.Converters;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class JsonIncomingMessage : IMessageIncomingBytes
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings();

        static JsonIncomingMessage()
        {
            JsonIncomingMessage.JsonSerializerSettings.Converters.Add(new JsonPacketConverter());
            JsonIncomingMessage.JsonSerializerSettings.Converters.Add(new JsonColorConverter());
        }

        public void Handle(ClientSession session, IncomingMessage message)
        {
            JsonPacket packet = JsonConvert.DeserializeObject<JsonPacket>(message.ReadStringUnsafe(), JsonIncomingMessage.JsonSerializerSettings);
            if (PlatformRacing3Server.PacketManager.GetIncomingJSONPacket(packet.Type, out IMessageIncomingJson handler))
            {
                handler.Handle(session, packet);
            }
            else
            {
                JsonIncomingMessage.Logger.Info("Unhandle packet id: " + packet.Type);
            }
        }
    }
}
