namespace SoupBinTCP.NET
{
    public interface IServerListener
    {
        /// <summary>
        /// Called when a client attempts to login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="requestedSession"></param>
        /// <param name="requestedSequenceNumber"></param>
        /// <returns>true if login is accepted, false otherwise</returns>
        LoginStatus OnLoginRequest(string username, string password, string requestedSession = "",
            ulong requestedSequenceNumber = 0);
        
    }
}