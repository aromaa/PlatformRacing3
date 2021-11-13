using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PlatformRacing3.Server.Host.Implementation;

internal sealed class ServerHostBuilder : IServerHostBuilder
{
	private readonly IHostBuilder builder;

	public ServerHostBuilder(IHostBuilder builder)
	{
		this.builder = builder;
	}

	public IServerHostBuilder ConfigureServices(Action<IConfiguration, IServiceCollection> configureServices)
	{
		this.builder.ConfigureServices((context, services) => configureServices(context.Configuration, services));

		return this;
	}
}