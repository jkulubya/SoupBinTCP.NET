using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET.Handlers
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
                msg.RequestedSequenceNumber, ctx.Channel.Id.AsLongText());
            if (result.Success)
            {
                ctx.Channel.Pipeline.Remove(this);
                ctx.Channel.Pipeline.Remove("LoginRequestFilter");
                ctx.Channel.Pipeline.AddLast(new ServerHandler(_listener));
                ctx.WriteAsync(new LoginAccepted(msg.RequestedSession, msg.RequestedSequenceNumber));
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
                ctx.WriteAsync(new LoginRejected(reason));
                ctx.CloseAsync();
            }
        }
        
        public override void ChannelActive(IChannelHandlerContext context)
        {
            _listener.OnSessionStart(context.Channel.Id.AsLongText());
        }


        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            context.Flush();
        }
    }
}