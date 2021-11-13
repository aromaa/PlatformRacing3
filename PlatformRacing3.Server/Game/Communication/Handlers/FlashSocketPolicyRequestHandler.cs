using System.Text;
using Net.Buffers;
using Net.Sockets;
using Net.Sockets.Pipeline.Handler;
using Net.Sockets.Pipeline.Handler.Incoming;

namespace PlatformRacing3.Server.Game.Communication.Handlers;

internal class FlashSocketPolicyRequestHandler : IncomingBytesHandler
{
	public static FlashSocketPolicyRequestHandler Instance { get; } = new FlashSocketPolicyRequestHandler();

	private static readonly ReadOnlyMemory<byte> FLASH_POLICY_REQUEST = Encoding.ASCII.GetBytes("<policy-file-request/>\0");
	private static readonly ReadOnlyMemory<byte> FLASH_POLICY_RESPONSE = Encoding.ASCII.GetBytes("<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\" /></cross-domain-policy>\0");

	protected override void Decode(IPipelineHandlerContext context, ref PacketReader reader)
	{
		if (reader.SequenceEqual(FlashSocketPolicyRequestHandler.FLASH_POLICY_REQUEST.Span))
		{
			_ = SendSocketPolicy(context.Socket);

			static async Task SendSocketPolicy(ISocket socket)
			{
				await socket.SendBytesAsync(FlashSocketPolicyRequestHandler.FLASH_POLICY_RESPONSE);

				socket.Disconnect("Socket policy request");
			}

			return;
		}

		//Only the first data may be the policy request, remove us after that's not the case
		context.Socket.Pipeline.RemoveHandler(this);
		context.Socket.Pipeline.Read(ref reader);
	}
}