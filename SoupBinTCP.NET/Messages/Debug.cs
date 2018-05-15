using System;
using System.Linq;
using System.Text;

namespace SoupBinTCP.NET.Messages
{
    public class Debug : Message
    {
        public string Text => Encoding.ASCII.GetString((Bytes.Skip(1).Take(Length - 1).ToArray()));

        public Debug(string text)
        {
            const char type = '+';
            var payload = type + text;
            Bytes = Encoding.ASCII.GetBytes(payload);
        }

        internal Debug(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}