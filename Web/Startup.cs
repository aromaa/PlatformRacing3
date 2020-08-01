using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Platform_Racing_3_Web.Middleware;

namespace Platform_Racing_3_Web
{
    internal class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;

                options.Cookie.Name = "PR3-Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.None; //None due to Chrome 71 breaking changes
                options.Cookie.Expiration = TimeSpan.FromDays(7);
            });

            services.AddMvc().AddXmlSerializerFormatters();
            services.AddCors();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Setup global stuff
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<CloudflareMiddleware>();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseCors(options =>
            {
                options.WithOrigins("http://pr3hub.com", "https://pr3hub.com").AllowAnyMethod().AllowCredentials();
            });

            app.UseMvc();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("I think you are lost...");
            });
        }
    }
}
