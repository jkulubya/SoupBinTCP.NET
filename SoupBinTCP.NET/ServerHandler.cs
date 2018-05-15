using System;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET
{
    internal class ServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var ba = new byte[256]; 
            if (message is IByteBuffer buffer)
            {
                //Buffer.BlockCopy(buffer.Array, 1, ba, 0, buffer.Array.Length - 1);
                //Console.WriteLine($"Received from client: {Encoding.UTF8.GetString(buffer.Array)}");
            }

            var debug = new Debug($"Echo - {Encoding.UTF8.GetString(ba)}");
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}