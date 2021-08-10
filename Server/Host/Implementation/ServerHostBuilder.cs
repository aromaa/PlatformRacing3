using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Platform_Racing_3_Server.Host.Implementation
{
	internal sealed class ServerHostBuilder : IServerHostBuilder
	{
		private readonly IHostBuilder builder;

		public ServerHostBuilder(IHostBuilder builder)
		{
			this.builder = builder;
		}

		public IServerHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
		{
			this.builder.ConfigureServices((context, services) => configureServices(services));

			return this;
		}
	}
}
