using Microsoft.AspNetCore.Authentication.Cookies;
using PlatformRacing3.Common.Campaign;
using PlatformRacing3.Common.Server;
using PlatformRacing3.Common.Utils;
using PlatformRacing3.Web.Middleware;

namespace PlatformRacing3.Web;

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
			options.Cookie.SameSite = SameSiteMode.None; //None due to Chrome 71 breaking changes
			options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
		});

		services.AddMvc().AddXmlSerializerFormatters();
		services.AddCors();

		services.AddSingleton<ServerManager>();
		services.AddSingleton<CampaignManager>();
	}
        
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		//Setup global stuff
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		LoggerUtil.LoggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

		//TODO: Eh...
		app.ApplicationServices.GetRequiredService<ServerManager>().LoadServersAsync().Wait();
		app.ApplicationServices.GetRequiredService<CampaignManager>().LoadCampaignTimesAsync().Wait();
		app.ApplicationServices.GetRequiredService<CampaignManager>().LoadPrizesAsync().Wait();

		app.UseMiddleware<CloudflareMiddleware>();
		app.UseRouting();
		app.UseCookiePolicy();
		app.UseAuthentication();
		app.UseCors(options =>
		{
			options.WithOrigins("http://pr3hub.com", "https://pr3hub.com").AllowAnyMethod().AllowCredentials();
		});

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}