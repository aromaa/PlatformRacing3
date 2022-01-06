using System.Text;
using Discord;
using Discord.Commands;
using PlatformRacing3.Common.Customization;
using PlatformRacing3.Common.Extensions;
using PlatformRacing3.Common.User;

namespace PlatformRacing3.Discord.Commands;

public class MyPartsCommand : ModuleBase<SocketCommandContext>
{
	[Command("pr3parts", ignoreExtraArgs: true)]
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

			EmbedBuilder embed = new();

			BuildHats(embed, userData);
			BuildParts(embed, userData);

			await this.ReplyAsync(message: this.Context.User.Mention, embed: embed.Build());

			static void BuildHats(EmbedBuilder embed, UserData userData)
			{
				PartStringBuilder builder = new();

				foreach (Hat hat in Enum.GetValues<Hat>())
				{
					if (hat == Hat.None || hat.IsStaffOnly())
					{
						continue;
					}

					builder.WriteConditional(hat, userData.HasHat(hat));
				}

				AddEmbedField(embed, builder, "Hats", inline: false);
			}

			static void BuildParts(EmbedBuilder embed, UserData userData)
			{
				(PartStringBuilder HeadBuilder, PartStringBuilder BodyBuilder, PartStringBuilder FeetBuilder) normalBuilder = (new PartStringBuilder(), new PartStringBuilder(), new PartStringBuilder());
				(PartStringBuilder HeadBuilder, PartStringBuilder BodyBuilder, PartStringBuilder FeetBuilder) tournamentBuilder = (new PartStringBuilder(), new PartStringBuilder(), new PartStringBuilder());

				foreach (Part part in Enum.GetValues<Part>())
				{
					if (part.IsStaffOnly())
					{
						continue;
					}
					
					ref (PartStringBuilder HeadBuilder, PartStringBuilder BodyBuilder, PartStringBuilder FeetBuilder) builder = ref part.IsTournamentPrize() ? ref tournamentBuilder : ref normalBuilder;

					(bool hasHead, bool hasBody, bool hasFeet) = part.HasParts();

					if (hasHead)
					{
						builder.HeadBuilder.WriteConditional(part, userData.HasHead(part));
					}

					if (hasBody)
					{
						builder.BodyBuilder.WriteConditional(part, userData.HasBody(part));
					}

					if (hasFeet)
					{
						builder.FeetBuilder.WriteConditional(part, userData.HasFeet(part));
					}
				}

				AddEmbedField(embed, normalBuilder.HeadBuilder, "Heads");
				AddEmbedField(embed, normalBuilder.BodyBuilder, "Bodies");
				AddEmbedField(embed, normalBuilder.FeetBuilder, "Feets");

				AddEmbedField(embed, tournamentBuilder.HeadBuilder, "Tournament Heads");
				AddEmbedField(embed, tournamentBuilder.BodyBuilder, "Tournament Bodies");
				AddEmbedField(embed, tournamentBuilder.FeetBuilder, "Tournament Feets");
			}

			static void AddEmbedField(EmbedBuilder embed, in PartStringBuilder builder, string name, bool inline = true)
			{
				embed.AddField(new EmbedFieldBuilder()
				{
					Name = $"{name} ({builder.Count}/{builder.Max})",
					Value = builder.Value,

					IsInline = inline
				});
			}
		}
	}

	private struct PartStringBuilder
	{
		private readonly StringBuilder stringBuilder;

		private int count;
		private int max;

		public PartStringBuilder()
		{
			this.stringBuilder = new StringBuilder();

			this.count = 0;
			this.max = 0;
		}

		internal readonly int Count => this.count;
		internal readonly int Max => this.max;

		internal readonly string Value => this.stringBuilder.ToString();

		internal void WriteConditional<T>(T value, bool condition) where T : struct, Enum
		{
			this.max++;

			this.stringBuilder.Append($"{value} ");

			if (condition)
			{
				this.stringBuilder.Append('✓');

				this.count++;
			}
			else
			{
				this.stringBuilder.Append('✕');
			}

			this.stringBuilder.AppendLine();
		}
	}
}