using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET.Codecs
{
    internal class SoupBinTcpMessageEncoder : MessageToMessageEncoder<Message>
    {
        protected override void Encode(IChannelHandlerContext context, Message message, List<object> output)
        {
            var msg = Unpooled.WrappedBuffer(message.Bytes);
            output.Add(msg);
        }
    }
}