using System.Data.Common;
using Microsoft.Extensions.Logging;
using PlatformRacing3.Common.Database;
using PlatformRacing3.Common.Server;
using PlatformRacing3.Common.User;
using PlatformRacing3.Server.Core;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Utils;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal sealed class GuestLoginIncomingMessage : IMessageIncomingJson
    {
        private readonly ServerManager serverManager;
        private readonly ClientManager clientManager;

        private readonly ILogger<GuestLoginIncomingMessage> logger;

        public GuestLoginIncomingMessage(ServerManager serverManager, ClientManager clientManager, ILogger<GuestLoginIncomingMessage> logger)
        {
            this.serverManager = serverManager;
            this.clientManager = clientManager;

            this.logger = logger;
        }

        public void Handle(ClientSession session, JsonPacket message)
        {
            if (session.UpgradeClientStatus(ClientStatus.LoggedIn))
            {
                DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"INSERT INTO base.users_logins(user_id, ip, data, type) VALUES(0, {session.IPAddres}, '', 'server') RETURNING id").ContinueWith((task) =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        DbDataReader reader = task.Result;
                        if (reader?.Read() ?? false)
                        {
                            uint id = (uint)(int)reader["id"];

                            session.UserData = new GuestUserData(id);
                            session.SendPacket(new LoginSuccessOutgoingMessage(session.SocketId, session.UserData.Id, session.UserData.Username, session.UserData.Permissions, session.UserData.GetVars("*")));

                            if (this.serverManager.TryGetServer(PlatformRacing3Server.ServerConfig.ServerId, out ServerDetails server))
                            {
                                session.UserData.SetServer(server);
                            }

                            this.clientManager.Add(session);
                        }
                        else
                        {
                            session.SendPacket(new LoginErrorOutgoingMessage("User data was not found"));
                        }
                    }
                    else if (task.IsFaulted)
                    {
                        this.logger.LogError(EventIds.GuestLoginFailed, task.Exception, "Failed to log login");

                        session.SendPacket(new LoginErrorOutgoingMessage("Critical error"));
                    }
                }));
            }
        }
    }
}
