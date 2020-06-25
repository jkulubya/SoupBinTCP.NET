using System;
using System.Buffers;

namespace SoupBinTCP.NET.Messages
{
    public class ServerHeartbeat : IMessage
    {
        public MessageType MessageType { get; } = MessageType.ServerHeartbeat;

        public ServerHeartbeat()
        {
        }

        internal ServerHeartbeat(ReadOnlySequence<byte> payload)
        {
        }

        byte[] IMessage.GetPayloadBytes() => Array.Empty<byte>();
    }
}
