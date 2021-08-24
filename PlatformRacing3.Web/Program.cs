using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Platform_Racing_3_Common.Campaign;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.Redis;
using Platform_Racing_3_Common.Server;
using Platform_Racing_3_Web.Config;
using Platform_Racing_3_Web.Controllers.DataAccess2;

namespace Platform_Racing_3_Web
{
    internal class Program
    {
        internal static WebConfig Config { get; private set; }
        
        internal static SmtpClient SmtpClient { get; private set; }

        private static void Main(string[] args)
        {
            Program.Config = JsonSerializer.Deserialize<WebConfig>(File.ReadAllText("settings.json"));

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
            
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build().Run();
        }
    }
}
