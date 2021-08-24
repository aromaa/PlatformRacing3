using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Platform_Racing_3_Server.Core;

namespace Platform_Racing_3_Server.Host.Implementation
{
	internal sealed class ServerHostService : IHostedService
	{
		private readonly PlatformRacing3Server server;

		public ServerHostService(PlatformRacing3Server server)
		{
			this.server = server;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await this.server.Init();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			this.server.Shutdown();

			return Task.CompletedTask;
		}
	}
}
