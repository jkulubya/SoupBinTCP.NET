using System;
using System.Buffers;

namespace SoupBinTCP.NET.Messages
{
    public class LoginRejected : IMessage
    {
        public MessageType MessageType { get; } = MessageType.LoginRejected;
        public RejectReason RejectReason { get; }

        public LoginRejected(RejectReason reason)
        {
            RejectReason = reason;
        }

        internal LoginRejected(ReadOnlySequence<byte> payload)
        {
        }

        byte[] IMessage.GetPayloadBytes()
        {
            throw new NotImplementedException();
        }
    }
}
