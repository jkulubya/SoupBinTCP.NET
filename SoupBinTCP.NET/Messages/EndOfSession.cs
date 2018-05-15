using System.Text;

namespace SoupBinTCP.NET.Messages
{
    public class EndOfSession : Message
    {
        public EndOfSession()
        {
            const char type = 'Z';
            Bytes = Encoding.ASCII.GetBytes(new[] {type});
        }

        internal EndOfSession(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}