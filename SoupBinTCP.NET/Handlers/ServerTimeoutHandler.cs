using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET.Handlers
{
    internal class ServerTimeoutHandler : ChannelDuplexHandler
    {
        private readonly IServerListener _listener;

        public ServerTimeoutHandler(IServerListener listener)
        {
            _listener = listener;
        }
        
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent e)
            {
                if (e.State == IdleState.WriterIdle)
                {
                    context.WriteAndFlushAsync(new ServerHeartbeat());
                }
                else if (e.State == IdleState.ReaderIdle)
                {
                    context.CloseAsync();
                    _listener.OnSessionEnd(context.Channel.Id.AsLongText());
                }
            }
        }

    }
}