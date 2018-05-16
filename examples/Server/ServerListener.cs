using System.Threading.Tasks;
using SoupBinTCP.NET;

namespace Server
{
    public class ServerListener : IServerListener
    {
        public LoginStatus OnLoginRequest(string username, string password, string requestedSession = "",
            ulong requestedSequenceNumber = 0)
        {
            // TODO fix this
            return new LoginStatus(false, RejectionReason.NotAuthorised);
        }

        public async Task OnServerListening()
        {
            throw new System.NotImplementedException();
        }

        public LoginStatus OnLoginRequest(string username, string password, string requestedSession, ulong requestedSequenceNumber,
            string channelId)
        {
            throw new System.NotImplementedException();
        }

        public async Task OnLogoutRequest(string channelId)
        {
            throw new System.NotImplementedException();
        }

        public async Task OnMessage(byte[] message, string channelId)
        {
            throw new System.NotImplementedException();
        }

        public async Task OnSessionEnd(string channelId)
        {
            throw new System.NotImplementedException();
        }

        public async Task OnServerDisconnect()
        {
            throw new System.NotImplementedException();
        }
    }
}