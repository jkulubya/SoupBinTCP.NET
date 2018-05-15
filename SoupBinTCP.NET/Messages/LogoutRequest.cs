using System.Text;

namespace SoupBinTCP.NET.Messages
{
    public class LogoutRequest : Message
    {
        public LogoutRequest()
        {
            const char type = 'O';
            Bytes = Encoding.ASCII.GetBytes(new[] {type});
        }
        
        internal LogoutRequest(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}