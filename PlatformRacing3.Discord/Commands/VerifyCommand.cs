using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PlatformRacing3.Common.User;

namespace PlatformRacing3.Discord.Commands
{
    public class VerifyCommand : ModuleBase<SocketCommandContext>
    {
        [Command("pr3verify")]
        [Summary("I swear I'm Jiggmin")]
        public Task VerifyAccount()
        {
            return Do();

            async Task Do()
            {
                uint userId = await UserManager.HasDiscordLinkage(this.Context.User.Id);
                if (userId == 0)
                {
                    await this.ReplyAsync($"{this.Context.User.Mention} link your Discord account with PR3 one here: https://api.pr3hub.com/linkdiscord");
                }
                else
                {
                    IRole role = this.Context.Guild.GetRole(595041143776083988);

                    await ((IGuildUser)this.Context.User).AddRoleAsync(role);
                    await this.ReplyAsync($"{this.Context.User.Mention} :sunglasses:");
                }
            }
        }
    }
}
