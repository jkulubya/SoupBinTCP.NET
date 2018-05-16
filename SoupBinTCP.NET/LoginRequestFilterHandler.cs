using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET
{
    internal class LoginRequestFilterHandler : SimpleChannelInboundHandler<LoginRequest>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, LoginRequest msg)
        {
            ctx.FireChannelRead(msg);
        }
    }
}