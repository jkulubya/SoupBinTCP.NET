namespace SoupBinTCP.NET.Messages
{
    public enum MessageType : byte
    {
        Debug = 0x2b, // "+"
        
        // Shared messages
        UnsequencedData = 0x55, // "U"
        
        // Server messages
         LoginAccepted = 0x41, // "A"
         LoginRejected = 0x4a, // "J"
         SequencedData = 0x53, // "S"
         ServerHeartbeat = 0x48, // "H"
         EndOfSession = 0x5a, // "Z"
        
        // Client messages
         LoginRequest = 0x4c, // "L"
         ClientHeartbeat = 0x52, // "R"
         LogoutRequest = 0x4f // "O"
    }
}