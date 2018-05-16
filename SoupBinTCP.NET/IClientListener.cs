using System;
using System.Threading.Tasks;

namespace SoupBinTCP.NET
{
    public interface IClientListener
    {
        Task OnConnect();
        Task OnMessage(byte[] message);
        Task OnLoginAccept(string session, ulong sequenceNumber);
        Task OnLoginReject();
        Task OnDisconnect();
    }
}