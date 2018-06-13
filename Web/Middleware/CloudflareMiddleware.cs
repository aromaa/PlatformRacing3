using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Middleware
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
