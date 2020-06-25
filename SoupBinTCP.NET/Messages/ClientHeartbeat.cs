using System;
using System.Buffers;

namespace SoupBinTCP.NET.Messages
{
    public class ClientHeartbeat : IMessage
    {
        public MessageType MessageType { get; } = MessageType.ClientHeartbeat;

        public ClientHeartbeat()
        {
        }

        internal ClientHeartbeat(ReadOnlySequence<byte> payload)
        {
        }

        byte[] IMessage.GetPayloadBytes() => Array.Empty<byte>();
    }
}
