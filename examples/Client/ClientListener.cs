using System.Threading.Tasks;
using SoupBinTCP.NET;

namespace Client
{
    public class ClientListener : IClientListener
    {
        public async Task OnConnect()
        {
            throw new System.NotImplementedException();
        }

        public async Task OnMessage(byte[] message)
        {
            throw new System.NotImplementedException();
        }

        public async Task OnLoginAccept(string session, ulong sequenceNumber)
        {
            throw new System.NotImplementedException();
        }

        public async Task OnLoginReject()
        {
            throw new System.NotImplementedException();
        }

        public async Task OnDisconnect()
        {
            throw new System.NotImplementedException();
        }
    }
}