using Net.Communication.Incoming.Handler;
using Net.Sockets.Pipeline.Handler;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Game.Communication.Messages.Incoming.Handlers;

internal abstract class AbstractIncomingClientSessionPacketHandler<T> : IClientSessionPacketHandler, IIncomingPacketHandler<T>
{
	public void Handle(IPipelineHandlerContext context, in T packet) => throw new NotSupportedException();

	internal abstract void Handle(ClientSession session, in T packet);
}