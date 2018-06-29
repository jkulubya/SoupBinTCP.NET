using System;
using System.Threading.Tasks;

namespace SoupBinTCP.NET
{
    public interface IClientListener
    {
        Task OnConnect();
        Task OnMessage(byte[] message);
        Task OnDebug(string message);
        Task OnLoginAccept(string session, ulong sequenceNumber);
        Task OnLoginReject(char rejectReasonCode);
        Task OnDisconnect();
    }
}