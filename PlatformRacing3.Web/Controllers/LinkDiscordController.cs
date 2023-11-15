using System.Text.Json;
using System.Text.Json.Serialization;
using Discord;
using Discord.Rest;
using Microsoft.AspNetCore.Mvc;
using PlatformRacing3.Common.User;
using PlatformRacing3.Web.Extensions;

namespace PlatformRacing3.Web.Controllers;

[ApiController]
[Route("linkdiscord")]
public class LinkDiscordController : ControllerBase
{
	private const string DISCORD_API_TOKEN = "https://discord.com/api/v10/oauth2/token";

	[HttpGet]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] //Dynamic get
	public async Task<ContentResult> GetAsync([FromQuery] string code)
	{
		uint userId = this.HttpContext.IsAuthenicatedPr3User();
		if (userId <= 0)
		{
			return this.Content(@"Please login to the client in the browser! Or <a href=""https://pr3hub.com/login"">click here</a> to do so without the Flash client.", "text/html");
		}

		if (string.IsNullOrWhiteSpace(code))
		{
			PlayerUserData playerUserData = await UserManager.TryGetUserDataByIdAsync(userId);

			return this.Content($@"Are you sure that you want to link your {playerUserData.Username} account to your Discord account?

<a href=""https://discordapp.com/api/oauth2/authorize?response_type=code&client_id={Program.Config.DiscordClientId}&scope=identify%20role_connections.write"">Click here to proceed</a>", "text/html");
		}
		else
		{
			DiscordTokenResponse tokenResponse = await RequestTokenAsync(code);
			if (!string.IsNullOrWhiteSpace(tokenResponse.Error))
			{
				return this.Content($"Requesting discord token returned the following error: {tokenResponse.ErrorReason}");
			}

			DiscordRestClient restClient = new(new DiscordRestConfig());
			await restClient.LoginAsync(Enum.Parse<TokenType>(tokenResponse.TokenType), tokenResponse.AccessToken);

			(uint linkedUserId, ulong linkedDiscordId) = await UserManager.HasDiscordLinkage(userId, restClient.CurrentUser.Id);
			if (linkedUserId == 0)
			{
				if (!await UserManager.TryInsertDiscordLinkage(userId, restClient.CurrentUser.Id))
				{
					return this.Content("Oops, something went wrong!");
				}
			}
			else
			{
				if (linkedUserId != userId || linkedDiscordId != restClient.CurrentUser.Id)
				{
					return this.Content("This Discord account has already been linked to different account!");
				}
			}

			PlayerUserData playerUserData = await UserManager.TryGetUserDataByIdAsync(userId, false);
			
			await restClient.ModifyUserApplicationRoleConnectionAsync(378590199527112704, new RoleConnectionProperties("Platform Racing 3: Reborn", playerUserData.Username, new Dictionary<string, string>
			{
				{ "rank", playerUserData.Rank.ToString() },
				{ "hats", playerUserData.HatsCount.ToString() },
				{ "permission_level", playerUserData.PermissionRank.ToString() },
				{ "first_login", playerUserData.Registered.ToString("o") },
				{ "last_login", (playerUserData.LastOnline ?? playerUserData.Registered).ToString("o") }
			}));

			return this.Content("All ok! :sunglasses:");
		}

		static async Task<DiscordTokenResponse> RequestTokenAsync(string code)
		{
			Dictionary<string, string> values = new()
			{
				{ "client_id", Program.Config.DiscordClientId },
				{ "client_secret", Program.Config.DiscordClientSecret },

				{ "grant_type", "authorization_code" },
				{ "scope", "identify role_connections.write" },

				{ "code", code },
			};

			using (HttpClient httpClient = new())
			{
				using (FormUrlEncodedContent content = new(values))
				{
					using (HttpResponseMessage message = await httpClient.PostAsync(LinkDiscordController.DISCORD_API_TOKEN, content))
					{
						return JsonSerializer.Deserialize<DiscordTokenResponse>(await message.Content.ReadAsStringAsync());
					}
				}
			}
		}
	}

	private class DiscordTokenResponse
	{
		[JsonPropertyName("token_type")]
		public string TokenType { get; set; }
		[JsonPropertyName("access_token")]
		public string AccessToken { get; set; }

		[JsonPropertyName("error")]
		public string Error { get; set; }
		[JsonPropertyName("error_description")]
		public string ErrorReason { get; set; }
	}
}