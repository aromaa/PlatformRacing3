using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.AspNetCore;
using PlatformRacing3.Common.Database;
using PlatformRacing3.Common.Redis;
using PlatformRacing3.Web.Config;
using PlatformRacing3.Web.Controllers.DataAccess2;

namespace PlatformRacing3.Web;

internal class Program
{
	internal static WebConfig Config { get; private set; }
        
	internal static SmtpClient SmtpClient { get; private set; }

	private static async Task Main(string[] args)
	{
		Program.Config = await JsonSerializer.DeserializeAsync<WebConfig>(File.OpenRead("settings.json"));

		DataAccess2.Init(Program.Config);

		DatabaseConnection.Init(Program.Config);
		RedisConnection.Init(Program.Config);
            
		Program.SmtpClient = new SmtpClient(Program.Config.SmtpHost, Program.Config.SmtpPort)
		{
			DeliveryMethod = SmtpDeliveryMethod.Network,
			EnableSsl = true,
			UseDefaultCredentials = false,
			Credentials = new NetworkCredential(Program.Config.SmtpUser, Program.Config.SmtpPass)
		};
            
		await WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build().RunAsync();
	}
}