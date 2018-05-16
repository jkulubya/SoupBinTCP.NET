using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET
{
    internal class ClientHandshakeHandler : ChannelHandlerAdapter
    {
        private readonly IClientListener _listener;
        private readonly LoginDetails _loginDetails;
        
        public ClientHandshakeHandler(LoginDetails loginDetails, IClientListener listener)
        {
            _listener = listener;
            _loginDetails = loginDetails;
        }
        
        public override void ChannelActive(IChannelHandlerContext context)
        {
            context.WriteAndFlushAsync(new LoginRequest(_loginDetails.Username, _loginDetails.Password,
                _loginDetails.RequestedSession, _loginDetails.RequestedSequenceNumber));
        }
    }
}