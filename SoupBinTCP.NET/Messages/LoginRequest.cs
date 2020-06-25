using System;
using System.Buffers;

namespace SoupBinTCP.NET.Messages
{
    public class LoginRequest : IMessage
    {
        public MessageType MessageType { get; } = MessageType.LoginRequest;

        public string Username { get; }
        public string Password { get; }
        public string RequestedSession { get; }
        public ulong RequestedSequenceNumber { get; }

        public LoginRequest(string username, string password, string requestedSession = "",
            ulong requestedSequenceNumber = 0)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username), "Username must be provided");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Password must be provided");
            }

            if (username.Length > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(username),
                    "Length of username parameter must be less than or equal to 6.");
            }

            if (password.Length > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(password),
                    "Length of password parameter must be less than or equal to 6.");
            }

            if (requestedSession.Length > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(requestedSession),
                    "Length of requestedSession parameter must be equal to 10.");
            }

            Username = username;
            Password = password;
            RequestedSession = requestedSession;
            RequestedSequenceNumber = requestedSequenceNumber;
        }

        internal LoginRequest(ReadOnlySequence<byte> payload)
        {
            throw new NotImplementedException();
        }

        byte[] IMessage.GetPayloadBytes()
        {
            throw new NotImplementedException();
        }
    }
}
