using System;
using System.Linq;
using System.Text;

namespace SoupBinTCP.NET.Messages
{
    public class LoginRequest : Message
    {
        public string Username => Encoding.ASCII.GetString(Bytes.Skip(1).Take(6).ToArray());
        public string Password => Encoding.ASCII.GetString(Bytes.Skip(7).Take(10).ToArray());
        public string RequestedSession => Encoding.ASCII.GetString(Bytes.Skip(17).Take(10).ToArray());
        public ulong RequestedSequenceNumber =>
            Convert.ToUInt64(Encoding.ASCII.GetString(Bytes.Skip(27).Take(20).ToArray()));
        
        public LoginRequest(string username, string password, string requestedSession = "", ulong requestedSequenceNumber = 0)
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

            const char type = 'L';
            var seqNo = requestedSequenceNumber.ToString().PadLeft(20);
            username = username.PadRight(6);
            password = password.PadRight(10);
            requestedSession = requestedSession.PadLeft(10);
            var payload = type + username + password + requestedSession + seqNo;
            Bytes = Encoding.ASCII.GetBytes(payload);
        }
        
        internal LoginRequest(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}