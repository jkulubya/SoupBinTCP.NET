using System;
using System.Buffers;

namespace SoupBinTCP.NET.Messages
{
    public class LogoutRequest : IMessage
    {
        public MessageType MessageType { get; } = MessageType.LogoutRequest;

        public LogoutRequest()
        {
        }

        internal LogoutRequest(ReadOnlySequence<byte> payload)
        {
        }

        byte[] IMessage.GetPayloadBytes() => Array.Empty<byte>();
    }
}
