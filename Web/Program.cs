using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.Redis;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Web.Config;

namespace Platform_Racing_3_Web
{
    internal class Program
    {
        internal static WebConfig Config { get; private set; }

        internal static ServerManager ServerManager { get; } = new ServerManager();

        internal static SmtpClient SmtpClient { get; private set; }

        private static void Main(string[] args)
        {
            try
            {
                XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), new FileInfo("log4net.config")); //Setup log4net logging
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to setup logging! {ex.ToString()}");
            }

            Program.Config = JsonConvert.DeserializeObject<WebConfig>(File.ReadAllText("settings.json"));

            DatabaseConnection.Init(Program.Config);
            RedisConnection.Init(Program.Config);

            _ = Program.ServerManager.LoadServersAsync(); //Load it async, continue other code

            Program.SmtpClient = new SmtpClient(Program.Config.SmtpHost, Program.Config.SmtpPort)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Program.Config.SmtpUser, Program.Config.SmtpPass)
            };

            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build().Run();
        }
    }
}
