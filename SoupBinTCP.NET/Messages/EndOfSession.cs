using System;
using System.Buffers;

namespace SoupBinTCP.NET.Messages
{
    public class EndOfSession : IMessage
    {
        public MessageType MessageType { get; } = MessageType.EndOfSession;

        public EndOfSession()
        {
        }

        internal EndOfSession(ReadOnlySequence<byte> payload)
        {
        }

        byte[] IMessage.GetPayloadBytes() => Array.Empty<byte>();
    }
}
