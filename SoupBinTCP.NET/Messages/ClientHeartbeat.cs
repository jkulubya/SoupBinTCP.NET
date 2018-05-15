using System;
using System.Text;

namespace SoupBinTCP.NET.Messages
{
    public class ClientHeartbeat : Message
    {
        public ClientHeartbeat()
        {
            const char type = 'R';
            Bytes = Encoding.ASCII.GetBytes(new[] {type});
        }

        internal ClientHeartbeat(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}