using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using PlatformRacing3.Common.User;
using PlatformRacing3.Common.Utils;
using PlatformRacing3.Server.Collections;
using PlatformRacing3.Server.Core;
using PlatformRacing3.Server.Game.Client;
using PlatformRacing3.Server.Game.Commands;
using PlatformRacing3.Server.Game.Communication.Messages.Incoming.Json;
using PlatformRacing3.Server.Game.Communication.Messages.Outgoing;
using PlatformRacing3.Server.Game.User.Identifiers;

namespace PlatformRacing3.Server.Game.Chat
{
	internal class ChatRoom
    {
        private const uint MAX_RECENT_MESSAGES = 25;

        private readonly ChatRoomManager chatRoomManager;
        private readonly CommandManager commandManager;

        internal ChatRoomType Type { get; }

        internal uint CreatorUserId { get; }
        [JsonPropertyName("creator")]
        internal string CreatorUsername => this.CreatorUserId == 0 ? null : UserManager.TryGetUserDataByIdAsync(this.CreatorUserId).Result?.Username ?? "Unknown";

        [JsonPropertyName("roomName")]
        internal string Name { get; }

        internal string Pass { get; set; }
        [JsonPropertyName("note")]
        internal string Note { get; set; }

        private ClientSessionCollection Clients;
        private ConcurrentBag<IUserIdentifier> BannedClients;

        [JsonPropertyName("members")]
        internal int MembersCount => this.Clients.Count;

        private ConcurrentQueue<ChatOutgoingMessage> RecentMessages;

        internal ChatRoom(ChatRoomManager chatRoomManager, CommandManager commandManager, ChatRoomType type, string name, string pass, string note) : this(chatRoomManager, commandManager, type, 0, name, pass, note)
        {
        }

        internal ChatRoom(ChatRoomManager chatRoomManager, CommandManager commandManager, ChatRoomType type, uint creatorUserId, string name, string pass, string note)
        {
            this.chatRoomManager = chatRoomManager;
            this.commandManager = commandManager;

            this.Clients = new ClientSessionCollection(removeCallback: this.Leave0);
            this.BannedClients = new ConcurrentBag<IUserIdentifier>();

            this.RecentMessages = new ConcurrentQueue<ChatOutgoingMessage>();

            this.CreatorUserId = creatorUserId;
            this.Type = type;
            this.Name = name;
            this.Pass = pass;
            this.Note = note;
        }

        internal ICollection<ClientSession> Members => this.Clients.Sessions;

        internal bool Join(ClientSession session, uint chatId = 0)
        {
            //First send the room vars
            session.SendPacket(new RoomVarsOutgoingMessage(chatId, this.Name, this.GetVars("creator", "note")));

            if (this.BannedClients.Any((i) => i.Matches(session.UserData.Id, session.SocketId, session.IPAddres)))
            {
                return false;
            }

            if (!this.Clients.TryAdd(session))
            {
                return false;
            }

            foreach(ClientSession other in this.Clients.Sessions)
            {
                other.TrackUserInRoom(this.Name, session.SocketId, session.UserData.Id, session.UserData.Username, session.UserData.GetVars("id"));
            }

            foreach(ChatOutgoingMessage message in this.RecentMessages)
            {
                session.SendPacket(message);
            }

            return true;
        }

        internal void Leave(ClientSession session) => this.Clients.TryRemove(session);

        public void Kick(ClientSession session)
        {
            this.Leave(session);
        }

        private void Leave0(ClientSession session)
        {
            foreach (ClientSession other in this.Clients.Sessions)
            {
                if (other != session)
                {
                    other.UntrackUserInRoom(this.Name, session.SocketId);
                }
            }

            if (this.Clients.Count <= 0)
            {
                this.chatRoomManager.Die(this);
            }
        }

        internal void HandleData(ClientSession session, JsonSendToRoomIncomingMessage.RoomMessageData data, bool sendToSelf = true)
        {
            if (this.Clients.Contains(session))
            {
                switch(data.Type)
                {
                    case "chat":
                        {
                            if (!session.IsGuest)
                            {
                                this.SendChatMessage(session, data.Data.GetProperty("message").GetString(), sendToSelf);
                            }
                        }
                        break;
                }
            }
        }

        private void SendChatMessage(ClientSession session, string message, bool sendToSelf = true)
        {
            message = message.Trim();

            if (message.StartsWith('/'))
            {
                string[] args = message[1..].Split(' ');

                if (!this.commandManager.Execte(session, args[0], args.AsSpan(start: 1, length: args.Length - 1)))
                {
                    session.SendPacket(new AlertOutgoingMessage("Unknown command"));
                }
            }
            else if (message.Length > 0)
            {
                ChatOutgoingMessage packet = new(this.Name, message, session.SocketId, session.UserData.Id, session.UserData.Username, session.UserData.NameColor);

                this.RecentMessages.Enqueue(packet);
                while (this.RecentMessages.Count > ChatRoom.MAX_RECENT_MESSAGES)
                {
                    this.RecentMessages.TryDequeue(out _);
                }

                if (sendToSelf)
                {
                    this.Clients.SendAsync(packet);
                }
                else
                {
                    this.Clients.SendAsync(packet, session);
                }

                if (this.Name == "chat-Home")
                {
                    PlatformRacing3Server.DiscordChatWebhook?.SendMessageAsync(text: $"`{message.Replace('`', '\'')}`", username: session.UserData.Username);
                }
            }
        }

        //Even tho the client has some funcitonality for this, its not properly supported, have some funcitonality for it tho
        internal void Ban(ClientSession session, ClientSession target)
        {
            if (session.IsLoggedIn && target.IsLoggedIn && !session.IsGuest && session.UserData.Id == this.CreatorUserId)
            {
                if (target.IsGuest)
                {
                    this.BannedClients.Add(new GuestIdentifier(target.SocketId, target.IPAddres));
                }
                else
                {
                    this.BannedClients.Add(new PlayerIdentifier(target.UserData.Id));
                }

                this.Kick(target); //BYE BYE!
            }
        }

        internal IReadOnlyDictionary<string, object> GetVars(params string[] vars) => JsonUtils.GetVars(this, vars);
        internal IReadOnlyDictionary<string, object> GetVars(HashSet<string> vars) => JsonUtils.GetVars(this, vars);
    }
}
