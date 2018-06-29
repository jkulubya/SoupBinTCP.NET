using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET.Handlers
{
    internal class LoginRequestFilterHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        {
            if (msg is LoginRequest)
            {
                ctx.FireChannelRead(msg);
            }
            else
            {
                ReferenceCountUtil.Release(msg);
            }

        }
    }
}