using System.Threading.Tasks;

namespace SoupBinTCP.NET
{
    public interface IServerListener
    {
        /// <summary>
        /// Server is ready to accept client connections
        /// </summary>
        /// <returns></returns>
        Task OnServerListening();

        /// <summary>
        /// Called when a client attempts to login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="requestedSession"></param>
        /// <param name="requestedSequenceNumber"></param>
        /// <param name="clientId"></param>
        LoginStatus OnLoginRequest(string username, string password, string requestedSession,
            ulong requestedSequenceNumber, string clientId);
        
        /// <summary>
        /// Client has requested to logout
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task OnLogout(string clientId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task OnDebug(string message, string clientId);
        
        /// <summary>
        /// Message received
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task OnMessage(byte[] message, string clientId);

        Task OnSessionStart(string sessionId);
        /// <summary>
        /// Client has logged out
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task OnSessionEnd(string clientId);
        
        /// <summary>
        /// After the server shuts down
        /// </summary>
        /// <returns></returns>
        Task OnServerDisconnect();
    }
}