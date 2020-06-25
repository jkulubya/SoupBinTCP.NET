using System;
using System.Buffers;

namespace SoupBinTCP.NET.Messages
{
    public class SequencedData : IMessage
    {
        public MessageType MessageType { get; } = MessageType.SequencedData;

        private readonly byte[] _message;
        public ReadOnlyMemory<byte> Message => _message;

        public SequencedData(byte[] message)
        {
            _message = message;
        }

        internal SequencedData(ReadOnlySequence<byte> payload)
        {
            throw new NotImplementedException();
        }

        byte[] IMessage.GetPayloadBytes()
        {
            throw new NotImplementedException();
        }
    }
}
