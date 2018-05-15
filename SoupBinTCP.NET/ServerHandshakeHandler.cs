using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET
{
    internal class ServerHandshakeHandler: SimpleChannelInboundHandler<LoginRequest>
    {
        private readonly IServerListener _listener;
        
        public ServerHandshakeHandler(IServerListener listener)
        {
            _listener = listener;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, LoginRequest msg)
        {
            var result = _listener.OnLoginRequest(msg.Username, msg.Password, msg.RequestedSession,
                msg.RequestedSequenceNumber);
            if (result.Success)
            {
                ctx.Channel.Pipeline.Remove(this);
                ctx.Channel.Pipeline.AddLast(new ServerTimeoutHandler(), new ServerHandler());
                ctx.WriteAndFlushAsync(new LoginAccepted(msg.RequestedSession, msg.RequestedSequenceNumber));
            }
            else
            {
                char reason;
                switch (result.RejectionReason)
                {
                        case RejectionReason.NotAuthorised:
                            reason = 'A';
                            break;
                        case RejectionReason.SessionNotAvailable:
                            reason = 'S';
                            break;
                        default:
                            reason = 'A';
                            break;
                }
                ctx.WriteAndFlushAsync(new LoginRejected(reason));
                ctx.CloseAsync();
            }
        }
    }
}