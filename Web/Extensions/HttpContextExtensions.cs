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

        private static readonly ISet<string> ALLOWED_DOMAINS = new HashSet<string>()
        {
            //The actual site
            "http://pr3hub.com",
            "https://pr3hub.com",

            //The Flash apps
            "app:/Platform Racing 3 Preloader.swf",
            "app:/Platform Racing 3 Client.swf",
            "app:/Platform%20Racing%203%20Preloader.swf",
            "app:/Platform%20Racing%203%20Client.swf",

            //Third party sites
            "http://jiggmin2.com",
            "https://jiggmin2.com",
            "https://discord.com",
        };

        internal static uint IsAuthenicatedPr3User(this HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("Referer", out StringValues referer) && !HttpContextExtensions.IsAllowed(referer))
            {
                return 0u; //Block possible bad request
            }

            if (httpContext.Request.Headers.TryGetValue("Origin", out StringValues origin) && !HttpContextExtensions.IsAllowed(origin))
            {
                return 0u; //Block possible bad request
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

        private static bool IsAllowed(string domain)
        {
            if (HttpContextExtensions.ALLOWED_DOMAINS.Contains(domain))
            {
                return true;
            }

            foreach(string allowedDomain in HttpContextExtensions.ALLOWED_DOMAINS)
            {
                if (domain.StartsWith(allowedDomain))
                {
                    return true;
                }
            }

            return false;
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
