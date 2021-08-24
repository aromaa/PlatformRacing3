using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace PlatformRacing3.Web.Middleware
{
    public class CloudflareMiddleware
    {
        private readonly RequestDelegate Next;

        public CloudflareMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("CF-Connecting-IP", out StringValues cloudFlareIpForward))
            {
                context.Connection.RemoteIpAddress = IPAddress.Parse(cloudFlareIpForward);
            }
            
            return this.Next(context);
        }
    }
}
