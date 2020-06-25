using System;
using System.Buffers;

namespace SoupBinTCP.NET.Messages
{
    public class Debug : IMessage
    {
        public MessageType MessageType { get; } = MessageType.Debug;

        public string Text { get; }

        public Debug(string text)
        {
            Text = text;
        }

        internal Debug(ReadOnlySequence<byte> payload)
        {
            throw new NotImplementedException();
        }

        byte[] IMessage.GetPayloadBytes()
        {
            throw new NotImplementedException();
        }
    }
}
