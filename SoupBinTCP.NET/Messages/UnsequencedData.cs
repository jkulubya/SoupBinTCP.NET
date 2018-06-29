using System;
using System.Linq;

namespace SoupBinTCP.NET.Messages
{
    public class UnsequencedData : Message
    {
        public byte[] Message => Bytes.Skip(1).Take(Length - 1).ToArray();
        
        public UnsequencedData(byte[] message)
        {
            const char type = 'U';
            var messageLength = message.Length;
            var payload = new byte[messageLength + 1];
            payload[0] = Convert.ToByte(type);
            Array.Copy(message, 0, payload, 1, messageLength);
            Bytes = payload;
        }
        
        internal UnsequencedData(byte[] bytes, bool addToBytesDirectly)
        {
            if (addToBytesDirectly)
            {
                Bytes = bytes;
            }
        }
    }
}