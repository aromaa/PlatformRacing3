using System.Text;
using Net.Buffers;
using Net.Sockets;
using Net.Sockets.Pipeline.Handler;
using Net.Sockets.Pipeline.Handler.Incoming;

namespace PlatformRacing3.Server.Game.Communication.Handlers;

internal sealed class FlashSocketPolicyRequestHandler : IncomingBytesHandler
{
	internal static FlashSocketPolicyRequestHandler StrictInstance { get; } = new(disconnectNoMatch: true);
	internal static FlashSocketPolicyRequestHandler LaxInstance { get; } = new(disconnectNoMatch: false);

	private static readonly ReadOnlyMemory<byte> FLASH_POLICY_REQUEST = Encoding.ASCII.GetBytes("<policy-file-request/>\0");
	private static readonly ReadOnlyMemory<byte> FLASH_POLICY_RESPONSE = Encoding.ASCII.GetBytes("<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\" /></cross-domain-policy>\0");

	private readonly bool disconnectNoMatch;

	private FlashSocketPolicyRequestHandler(bool disconnectNoMatch)
	{
		this.disconnectNoMatch = disconnectNoMatch;
	}

	protected override void Decode(IPipelineHandlerContext context, ref PacketReader reader)
	{
		ISocket socket = context.Socket;

		if (reader.SequenceEqual(FlashSocketPolicyRequestHandler.FLASH_POLICY_REQUEST.Span))
		{
			ValueTask task = socket.SendBytesAsync(FlashSocketPolicyRequestHandler.FLASH_POLICY_RESPONSE);
			if (task.IsCompletedSuccessfully)
			{
				socket.Disconnect("Socket policy request");
			}
			else
			{
				socket.Pipeline.RemoveHandler(this);

				task.AsTask().ContinueWith(_ => socket.Disconnect("Socket policy request"));
			}
		}
		else if (this.disconnectNoMatch)
		{
			socket.Disconnect("No socket policy request");
		}
		else
		{
			//Only the first data may be the policy request, remove us after that's not the case
			socket.Pipeline.RemoveHandler(this);
			socket.Pipeline.Read(ref reader);
		}
	}
}