namespace SoupBinTCP.NET
{
    public class LoginStatus
    {
        public bool Success { get; }
        public RejectionReason RejectionReason { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="success">True when the login operation is a success, false otherwise</param>
        /// <param name="rejectionReason">True if not authorised (username/password combination wrong). False if requested session is invalid or unavailable</param>
        public LoginStatus(bool success, RejectionReason rejectionReason)
        {
            Success = success;
            RejectionReason = rejectionReason;
        }
    }

    public enum RejectionReason
    {
        NotAuthorised = 1,
        SessionNotAvailable = 2
    }
}