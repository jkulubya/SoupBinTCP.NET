using System;
using System.Buffers;

namespace SoupBinTCP.NET.Messages
{
    public class UnsequencedData : IMessage
    {
        public MessageType MessageType { get; }

        private readonly byte[] _message;
        public ReadOnlyMemory<byte> Message => _message;

        public UnsequencedData(byte[] message)
        {
            _message = message;
        }

        internal UnsequencedData(ReadOnlySequence<byte> payload)
        {
            throw new NotImplementedException();
        }

        byte[] IMessage.GetPayloadBytes()
        {
            throw new NotImplementedException();
        }
    }
}
