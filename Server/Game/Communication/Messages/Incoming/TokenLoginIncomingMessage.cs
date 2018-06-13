using log4net;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.Redis;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Server.Core;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing;
using Platform_Racing_3_Server.Game.Communication.Messages.Outgoing.Json;
using Platform_Racing_3_Server.Net;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class TokenLoginIncomingMessage : MessageIncomingJson<JsonTokenLoginIncomingMessage>
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

                                                PlatformRacing3Server.ClientManager.Add(session);

                                                //Race condition
                                                if (PlatformRacing3Server.ServerManager.TryGetServer(PlatformRacing3Server.ServerConfig.ServerId, out ServerDetails server))
                                                {
                                                    result_.SetServer(server);
                                                }
                                            }
                                            else if (task__.IsFaulted)
                                            {
                                                TokenLoginIncomingMessage.Logger.Error($"Failed to insert login", task.Exception);

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
                                    session.SendPacket(new LoginErrorOutgoingMessage("Critical error while trying to load user data"));

                                    TokenLoginIncomingMessage.Logger.Error("Failed to load user data", task_.Exception);
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
                        session.SendPacket(new LoginErrorOutgoingMessage("Critical error while trying to retrieve login token information"));

                        TokenLoginIncomingMessage.Logger.Error("Failed to retieve login token", task.Exception);
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
