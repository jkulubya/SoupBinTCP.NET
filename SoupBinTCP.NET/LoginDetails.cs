namespace SoupBinTCP.NET
{
    public class LoginDetails
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RequestedSession { get; set; } = "";
        public ulong RequestedSequenceNumber { get; set; } = 0;
    }
}