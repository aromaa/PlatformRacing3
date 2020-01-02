using Discord.Commands;
using Discord.Commands.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Platform_Racing_3_Common.Server;

namespace Discord_Bot.Commands
{
    public class OnlineCommand : ModuleBase<SocketCommandContext>
    {
        private ServerManager ServerManager;

        public OnlineCommand(ServerManager serverManager)
        {
            this.ServerManager = serverManager;
        }

        [Command("pr3online")]
        [Summary("Returns server status and active player count.")]
        public Task GetOnlinePlayersCount()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (ServerDetails server in this.ServerManager.GetServers())
            {
                stringBuilder.Append($"**{server.Name}**");
                stringBuilder.Append(" - ");
                stringBuilder.Append(server.Status);
                stringBuilder.AppendLine();
            }

            EmbedBuilder embed = new EmbedBuilder();

            embed.WithTitle("Server Status");
            embed.WithColor(Color.Blue);
            embed.WithAuthor(author = > { author.WithName(this.Context.User.Username + this.Context.User.Discriminator).WithIconUrl(this.Context.User.GetAvatarUrl()) });

            embed.AddField(new EmbedFieldBuilder()
            {
                Value = stringBuilder.ToString()
            });

            await this.ReplyAsync(embed: embed.Build());
        }
    }
}
