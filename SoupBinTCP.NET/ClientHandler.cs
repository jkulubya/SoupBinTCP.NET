using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET
{
    internal class ClientHandler : MessageToMessageEncoder<Message>
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var ba = new byte[256]; 

            if (message is IByteBuffer buffer)
            {
                Buffer.BlockCopy(buffer.Array, 1, ba, 0, buffer.Array.Length - 1);
                Console.WriteLine($"{Encoding.UTF8.GetString(ba)}");
            }
        }

        protected override void Encode(IChannelHandlerContext context, Message message, List<object> output)
        {
            var msg = Unpooled.WrappedBuffer(message.Bytes);
            output.Add(msg);
        }
    }
}