using System;

namespace SoupBinTCP.NET.Messages
{
    public class SequencedData : Message
    {
        public SequencedData(byte[] message)
        {
            const char type = 'S';
            var messageLength = message.Length;
            var payload = new byte[messageLength + 1];
            payload[0] = Convert.ToByte(type);
            Array.Copy(message, 0, payload, 1, messageLength);
            Bytes = payload;
        }
        
        internal SequencedData(byte[] bytes, bool addToBytesDirectly)
        {
            if (addToBytesDirectly)
            {
                Bytes = bytes;
            }
        }
    }
}