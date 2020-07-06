using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Platform_Racing_3_Common.User;
using Platform_Racing_3_Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Controllers
{
    [Route("linkdiscord")]
    public class LinkDiscordController : Controller
    {
        private const string DISCORD_API_TOKEN = "https://discord.com/api/v6/oauth2/token";
        private const string DISCORD_API_ME = "https://discord.com/api/v6/users/@me";

        [HttpGet]
        public async Task<ContentResult> GetAsync([FromQuery] string code)
        {
            uint userId = this.HttpContext.IsAuthenicatedPr3User();
            if (userId > 0)
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    PlayerUserData playerUserData = await UserManager.TryGetUserDataByIdAsync(userId);

                    return this.Content($@"Are you sure you want to link your {playerUserData.Username} account to your Discord account?

<a href=""https://discordapp.com/api/oauth2/authorize?response_type=code&client_id={Program.Config.DiscordClientId}&scope=identify"">Click here to proceed</a>", "text/html");
                }
                else
                {
                    DiscordTokenResponse tokenResponse = await RequestTokenAsync();
                    if (!string.IsNullOrWhiteSpace(tokenResponse.Error))
                    {
                        return this.Content($"Requesting discord token returned the following error: {tokenResponse.ErrorReason}");
                    }

                    DiscordUserResponse user = await GetUser();
                    if (user.Code != null)
                    {
                        return this.Content($"Requesting discord profile resulted to the following error: {user.Message}");
                    }

                    uint linkedToAccount = await UserManager.HasDiscordLinkage(userId, user.Id);
                    if (linkedToAccount == 0)
                    {
                        if (!await UserManager.TryInsertDiscordLinkage(userId, user.Id))
                        {
                            return this.Content("Oops, something went wrong!");
                        }

                        return this.Content("All ok! :sunglasses:");
                    }
                    else
                    {
                        if (linkedToAccount == userId)
                        {
                            return this.Content("This account has already been linked to Discord account!");
                        }
                        else
                        {
                            return this.Content("This Discord account has already been linked to different account!");
                        }
                    }

                    async Task<DiscordUserResponse> GetUser()
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenResponse.TokenType, tokenResponse.AccessToken);

                            using (HttpResponseMessage message = await httpClient.GetAsync(LinkDiscordController.DISCORD_API_ME))
                            {
                                return JsonConvert.DeserializeObject<DiscordUserResponse>(await message.Content.ReadAsStringAsync());
                            }
                        }
                    }

                    async Task<DiscordTokenResponse> RequestTokenAsync()
                    {
                        IDictionary<string, string> values = new Dictionary<string, string>()
                        {
                            { "client_id", Program.Config.DiscordClientId },
                            { "client_secret", Program.Config.DiscordClientSecret },
                            { "grant_type", "authorization_code" },
                            { "code", code },
                            { "scope", "identify" },
                        };

                        using (HttpClient httpClient = new HttpClient())
                        {
                            using (FormUrlEncodedContent content = new FormUrlEncodedContent(values))
                            {
                                using (HttpResponseMessage message = await httpClient.PostAsync(LinkDiscordController.DISCORD_API_TOKEN, content))
                                {
                                    return JsonConvert.DeserializeObject<DiscordTokenResponse>(await message.Content.ReadAsStringAsync());
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return this.Content("Please login to the client!");
            }
        }

        private class DiscordTokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }

            [JsonProperty("error_description")]
            public string ErrorReason { get; set; }
        }

        private class DiscordUserResponse
        {
            [JsonProperty("id")]
            public ulong Id { get; set; }

            [JsonProperty("code")]
            public uint? Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }
    }
}
