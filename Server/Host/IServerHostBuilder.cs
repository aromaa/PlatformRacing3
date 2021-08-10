using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Platform_Racing_3_Server.Host
{
	public interface IServerHostBuilder
	{
		public IServerHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);
	}
}
