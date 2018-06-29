using System;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET.Handlers
{
    internal class ServerHandler : ChannelHandlerAdapter
    {
        private readonly IServerListener _listener;

        public ServerHandler(IServerListener listener)
        {
            _listener = listener;
        }
        
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            switch (message)
            {
                case Debug msg:
                    _listener.OnDebug(msg.Text, context.Channel.Id.AsLongText());
                    break;
                case LogoutRequest msg:
                    _listener.OnLogout(context.Channel.Id.AsLongText());
                    context.CloseAsync();
                    break;
                case UnsequencedData msg:
                    _listener.OnMessage(msg.Message, context.Channel.Id.AsLongText());
                    break;
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            _listener.OnSessionEnd(context.Channel.Id.AsLongText());
        }

        //public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            // TODO add proper logging
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}