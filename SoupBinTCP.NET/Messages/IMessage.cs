namespace SoupBinTCP.NET.Messages
{
    public interface IMessage
    {
        public MessageType MessageType { get; }
        internal byte[] GetPayloadBytes();
    }
}
