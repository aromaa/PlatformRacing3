using Microsoft.Extensions.Logging;
using PlatformRacing3.Common.Database;
using PlatformRacing3.Common.Redis;
using PlatformRacing3.Common.Server;
using PlatformRacing3.Common.User;
using PlatformRacing3.Server.Core;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Utils;
using StackExchange.Redis;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming
{
    internal sealed class TokenLoginIncomingMessage : MessageIncomingJson<JsonTokenLoginIncomingMessage>
    {
        private readonly ServerManager serverManager;
        private readonly ClientManager clientManager;

        private readonly ILogger<TokenLoginIncomingMessage> logger;

        public TokenLoginIncomingMessage(ServerManager serverManager, ClientManager clientManager, ILogger<TokenLoginIncomingMessage> logger)
        {
            this.serverManager = serverManager;
            this.clientManager = clientManager;

            this.logger = logger;
        }

        internal override void Handle(ClientSession session, JsonTokenLoginIncomingMessage message)
        {
            if (session.UpgradeClientStatus(ClientStatus.LoggedIn))
            {
                RedisConnection.GetDatabase().StringGetAsync($"logintoken:{message.LoginToken}").ContinueWith((task) =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        RedisValue result = task.Result;
                        if (result.HasValue)
                        {
                            UserManager.TryGetUserDataByIdAsync((uint)result, false).ContinueWith((task_) =>
                            {
                                if (task_.IsCompletedSuccessfully)
                                {
                                    PlayerUserData result_ = task_.Result;
                                    if (result_ != null)
                                    {
                                        DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"INSERT INTO base.users_logins(user_id, ip, data, type) VALUES({result_.Id}, {session.IPAddres}, {message.LoginToken}, 'server')").ContinueWith((task__) =>
                                        {
                                            if (task__.IsCompletedSuccessfully)
                                            {
                                                session.UserData = result_;
                                                session.SendPacket(new LoginSuccessOutgoingMessage(session.SocketId, result_.Id, result_.Username, result_.Permissions, result_.GetVars("*")));
                                                session.SendPacket(new FriendsAndIgnoredOutgoingMessage(result_.Friends, result_.Ignored));

                                                this.clientManager.Add(session);

                                                //Race condition
                                                if (this.serverManager.TryGetServer(PlatformRacing3Server.ServerConfig.ServerId, out ServerDetails server))
                                                {
                                                    result_.SetServer(server);
                                                }
                                            }
                                            else if (task__.IsFaulted)
                                            {
                                                this.logger.LogError(EventIds.TokenLoginFailed, task__.Exception, "Failed to insert login");

                                                session.SendPacket(new LoginErrorOutgoingMessage("Critical error"));
                                            }
                                        }));
                                    }
                                    else
                                    {
                                        session.SendPacket(new LoginErrorOutgoingMessage("User data was not found"));
                                    }
                                }
                                else if (task_.IsFaulted)
                                {
	                                this.logger.LogError(EventIds.TokenLoginFailed, task_.Exception, "Failed to load user data");

                                    session.SendPacket(new LoginErrorOutgoingMessage("Critical error while trying to load user data"));
                                }
                                else
                                {
                                    session.SendPacket(new LoginErrorOutgoingMessage("Failed to load user data"));
                                }
                            });
                        }
                        else
                        {
                            session.SendPacket(new LoginErrorOutgoingMessage("This login token is invalid"));
                        }
                    }
                    else if (task.IsFaulted)
                    {
	                    this.logger.LogError(EventIds.TokenLoginFailed, task.Exception, "Failed to retrieve login token");

                        session.SendPacket(new LoginErrorOutgoingMessage("Critical error while trying to retrieve login token information"));

                    }
                    else
                    {
                        session.SendPacket(new LoginErrorOutgoingMessage("Failed to retrieve login token information"));
                    }
                });
            }
        }
    }
}
