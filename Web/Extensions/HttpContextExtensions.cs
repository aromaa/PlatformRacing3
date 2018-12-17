using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Platform_Racing_3_Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Extensions
{
    internal static class HttpContextExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string AUTHENICATION_TYPE = "Login";
        private const string AUTHENICATION_IDENTITY = "PlatformRacing3Identity";

        private const string HARDCORED_DOMAIN_BCS_IM_BAD = "pr3hub.com";

        internal static uint IsAuthenicatedPr3User(this HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("Referer", out StringValues referer))
            {
                //TODO: FIX THIS WTF

                string refererHost = referer;
                if (!refererHost.StartsWith("http://" + HttpContextExtensions.HARDCORED_DOMAIN_BCS_IM_BAD) && !refererHost.StartsWith("https://" + HttpContextExtensions.HARDCORED_DOMAIN_BCS_IM_BAD))
                {
                    int indexOf = refererHost.IndexOf("." + HttpContextExtensions.HARDCORED_DOMAIN_BCS_IM_BAD);
                    if (indexOf == -1 || (refererHost.Length >= indexOf + ("." + HttpContextExtensions.HARDCORED_DOMAIN_BCS_IM_BAD).Length + 1 && refererHost[indexOf + ("." + HttpContextExtensions.HARDCORED_DOMAIN_BCS_IM_BAD).Length] == '.'))
                    {
                        return 0u; //Block possible bad request
                    }
                }
            }

            if (httpContext.Request.Headers.TryGetValue("Origin", out StringValues origin))
            {
                //TODO: FIX THIS WTF

                string originHost = origin;
                if (!originHost.StartsWith("http://" + HttpContextExtensions.HARDCORED_DOMAIN_BCS_IM_BAD) && !originHost.StartsWith("https://" + HttpContextExtensions.HARDCORED_DOMAIN_BCS_IM_BAD))
                {
                    int indexOf = originHost.IndexOf("." + HttpContextExtensions.HARDCORED_DOMAIN_BCS_IM_BAD);
                    if (indexOf == -1 || (originHost.Length >= indexOf + ("." + HttpContextExtensions.HARDCORED_DOMAIN_BCS_IM_BAD).Length + 1 && originHost[indexOf + ("." + HttpContextExtensions.HARDCORED_DOMAIN_BCS_IM_BAD).Length] == '.'))
                    {
                        return 0u; //Block possible bad request
                    }
                }
            }

            ClaimsPrincipal claimsPrincipal = httpContext.User;
            if (claimsPrincipal != null)
            {
                IIdentity identity = claimsPrincipal.Identity;
                if (identity != null && identity.IsAuthenticated && identity.Name == HttpContextExtensions.AUTHENICATION_IDENTITY)
                {
                    if (uint.TryParse(claimsPrincipal.FindFirstValue(ClaimTypes.Sid), out uint userId))
                    {
                        return userId;
                    }
                }
            }

            return 0u;
        }

        internal static Task AuthenicatePr3UserAsync(this HttpContext httpContext, uint userId)
        {
            return DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ReadDataAsync($"INSERT INTO base.users_logins(user_id, ip, data, type) VALUES({userId}, {httpContext.Connection.RemoteIpAddress}, '', 'web')").ContinueWith(async (task) =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(HttpContextExtensions.AUTHENICATION_TYPE);
                    identity.AddClaim(new Claim(ClaimTypes.Name, HttpContextExtensions.AUTHENICATION_IDENTITY));
                    identity.AddClaim(new Claim(ClaimTypes.Sid, userId.ToString(), ClaimValueTypes.UInteger32));

                    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties()
                    {
                        IsPersistent = true,
                    });
                }
                else if (task.IsFaulted)
                {
                    HttpContextExtensions.Logger.Error("Failed to insert login", task.Exception);
                }
            }));
        }

        internal static Task LogOutPr3UserAsync(this HttpContext httpContext)
        {
            return httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
