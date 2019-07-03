using Discord;
using Discord.Commands;
using Platform_Racing_3_Common.Customization;
using Platform_Racing_3_Common.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.Commands
{
    public class MyPartsCommand : ModuleBase<SocketCommandContext>
    {
        [Command("myparts")]
        [Summary("PARTS! PARTS! WHICH ONE AM I MISSING!?")]
        public Task GetOnlinePlayersCount()
        {
            return DoAsync();

            async Task DoAsync()
            {
                uint userId = await UserManager.HasDiscordLinkage(this.Context.User.Id);
                if (userId == 0)
                {
                    await this.ReplyAsync("Well bruh, u ain't got ur Discord linkage");

                    return;
                }

                PlayerUserData userData = await UserManager.TryGetUserDataByIdAsync(userId);

                StringBuilder hats = new StringBuilder();
                foreach (Hat hat in Enum.GetValues(typeof(Hat)))
                {
                    if (hat == Hat.None)
                    {
                        continue;
                    }

                    hats.Append(hat.ToString());
                    hats.Append(" ");

                    if (userData.HasHat(hat))
                    {
                        hats.Append("✓");
                    }
                    else
                    {
                        hats.Append("✕");
                    }

                    hats.AppendLine();
                }

                StringBuilder heads = new StringBuilder();
                StringBuilder bodies = new StringBuilder();
                StringBuilder feets = new StringBuilder();
                foreach (Part part in Enum.GetValues(typeof(Part)))
                {
                    if (part == Part.None)
                    {
                        continue;
                    }

                    AppendToAll(part.ToString());
                    AppendToAll(" ");

                    if (userData.HasHead(part))
                    {
                        heads.Append("✓");
                    }
                    else
                    {
                        heads.Append("✕");
                    }

                    if (userData.HasBody(part))
                    {
                        bodies.Append("✓");
                    }
                    else
                    {
                        bodies.Append("✕");
                    }

                    if (userData.HasFeet(part))
                    {
                        feets.Append("✓");
                    }
                    else
                    {
                        feets.Append("✕");
                    }

                    BreakAll();

                    void AppendToAll(string value)
                    {
                        heads.Append(value);
                        bodies.Append(value);
                        feets.Append(value);
                    }

                    void BreakAll()
                    {
                        heads.AppendLine();
                        bodies.AppendLine();
                        feets.AppendLine();
                    }
                }

                EmbedBuilder embed = new EmbedBuilder();
                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Hats",
                    Value = hats.ToString()
                });

                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Heads",
                    Value = heads.ToString(),

                    IsInline = true
                });

                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Bodies",
                    Value = bodies.ToString(),

                    IsInline = true
                });

                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Feets",
                    Value = feets.ToString(),

                    IsInline = true
                });

                await this.ReplyAsync(message: this.Context.User.Mention, embed: embed.Build());
            }
        }
    }
}
