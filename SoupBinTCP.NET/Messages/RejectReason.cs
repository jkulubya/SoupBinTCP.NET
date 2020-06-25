namespace SoupBinTCP.NET.Messages
{
    public enum RejectReason : byte
    {
        NotAuthorized = 0x41,
        SessionNotAvailable = 0x53
    }
}
