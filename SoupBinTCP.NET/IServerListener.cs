using System.Threading.Tasks;

namespace SoupBinTCP.NET
{
    public interface IServerListener
    {
        Task OnServerListening();
        
        /// <summary>
        /// Called when a client attempts to login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="requestedSession"></param>
        /// <param name="requestedSequenceNumber"></param>
        /// <returns>true if login is accepted, false otherwise</returns>
        LoginStatus OnLoginRequest(string username, string password, string requestedSession,
            ulong requestedSequenceNumber, string channelId);
        Task OnLogoutRequest(string channelId);
        Task OnMessage(byte[] message, string channelId);
        Task OnSessionEnd(string channelId);
        Task OnServerDisconnect();
    }
}