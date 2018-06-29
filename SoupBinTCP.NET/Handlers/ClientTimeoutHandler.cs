using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET.Handlers
{
    internal class ClientTimeoutHandler : ChannelDuplexHandler
    {
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent e)
            {
                if (e.State == IdleState.WriterIdle)
                {
                    context.WriteAndFlushAsync(new ClientHeartbeat());
                }
                else if(e.State == IdleState.ReaderIdle)
                {
                    context.CloseAsync();
                }
            }
        }
    }
}