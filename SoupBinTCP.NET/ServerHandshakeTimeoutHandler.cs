using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET
{
    internal class ServerHandshakeTimeoutHandler: ChannelHandlerAdapter
    {
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent e)
            {
                if(e.State == IdleState.ReaderIdle)
                {
                    context.CloseAsync();
                }
            }
        }
    }
}