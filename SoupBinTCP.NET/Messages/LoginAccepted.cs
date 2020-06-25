using System;
using System.Buffers;

namespace SoupBinTCP.NET.Messages
{
    public class LoginAccepted : IMessage
    {
        public MessageType MessageType { get; } = MessageType.LoginAccepted;
        public string Session { get; }
        public ulong SequenceNumber { get; }

        public LoginAccepted(string session, ulong sequenceNumber)
        {
            if (session.Length > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(session), "Session must have maximum length 10");
            }

            Session = session;
            SequenceNumber = sequenceNumber;
        }

        internal LoginAccepted(ReadOnlySequence<byte> payload)
        {
            throw new NotImplementedException();
        }

        byte[] IMessage.GetPayloadBytes()
        {
            throw new NotImplementedException();
        }
    }
}
