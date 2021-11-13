using System.Text;
using Discord.Commands;
using PlatformRacing3.Common.Server;

namespace PlatformRacing3.Discord.Commands
{
	public class OnlineCommand : ModuleBase<SocketCommandContext>
    {
        private ServerManager ServerManager;

        public OnlineCommand(ServerManager serverManager)
        {
            this.ServerManager = serverManager;
        }

        [Command("pr3online")]
        [Summary("Whos online? Log in")]
        public Task GetOnlinePlayersCount()
        {
            StringBuilder stringBuilder = new(this.Context.User.Mention);
            stringBuilder.AppendLine();

            foreach (ServerDetails server in this.ServerManager.GetServers())
            {
                stringBuilder.Append(server.Name);
                stringBuilder.Append(": ");
                stringBuilder.Append(server.Status);
                stringBuilder.AppendLine();
            }

            return this.ReplyAsync(stringBuilder.ToString());
;        }
    }
}
