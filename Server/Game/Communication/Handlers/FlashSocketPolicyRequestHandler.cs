using Net.Communication.Incoming.Handlers;
using Net.Communication.Pipeline;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Handlers
{
    internal class FlashSocketPolicyRequestHandler : IIncomingObjectHandler<ReadOnlySequence<byte>>
    {
        public static FlashSocketPolicyRequestHandler Instance { get; } = new FlashSocketPolicyRequestHandler();

        private static readonly ReadOnlyMemory<byte> FLASH_POLICY_REQUEST = Encoding.ASCII.GetBytes("<policy-file-request/>\0");
        private static readonly ReadOnlyMemory<byte> FLASH_POLICY_RESPONSE = Encoding.ASCII.GetBytes("<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\" /></cross-domain-policy>\0");

        public void Handle(ref SocketPipelineContext context, ref ReadOnlySequence<byte> data)
        {
            if (data.Length == FlashSocketPolicyRequestHandler.FLASH_POLICY_REQUEST.Length)
            {
                //Single segment can be optimized
                if (data.IsSingleSegment)
                {
                    if (data.First.Span.SequenceEqual(FlashSocketPolicyRequestHandler.FLASH_POLICY_REQUEST.Span))
                    {
                        context.Connection.Send(FlashSocketPolicyRequestHandler.FLASH_POLICY_RESPONSE);
                        context.Connection.Disconnect("Socket policy request (Fast path)");

                        data = data.Slice(data.End);

                        return;
                    }
                }
                else
                {
                    //Small enough, copy to stack and read them
                    Span<byte> bytes = stackalloc byte[FlashSocketPolicyRequestHandler.FLASH_POLICY_REQUEST.Length];

                    data.CopyTo(bytes);

                    if (bytes.SequenceEqual(FlashSocketPolicyRequestHandler.FLASH_POLICY_REQUEST.Span))
                    {
                        context.Connection.Send(FlashSocketPolicyRequestHandler.FLASH_POLICY_RESPONSE);
                        context.Connection.Disconnect("Socket policy request (Slow path)");

                        data = data.Slice(data.End);

                        return;
                    }
                }
            }

            //Only the first data may be the policy request, remove us after thats not the case
            context.RemoveHandler(this);
            context.ProgressReadHandler(ref data);
        }
    }
}
