using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PlatformRacing3.Server.Host
{
	public interface IServerHostBuilder
	{
		public IServerHostBuilder ConfigureServices(Action<IConfiguration, IServiceCollection> configureServices);
	}
}
