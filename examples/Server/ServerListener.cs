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
    }
}