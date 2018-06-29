using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;
using Debug = SoupBinTCP.NET.Messages.Debug;

namespace SoupBinTCP.NET.Codecs
{
    internal class SoupBinTcpMessageDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            if(!input.IsReadable()) return;
            
            var bytes = new byte[input.ReadableBytes];
            input.ReadBytes(bytes);
            var type = Convert.ToChar(bytes[0]);
            switch (type)
            {
                case '+':
                    output.Add(new Debug(bytes));
                    break;
                case 'A':
                    output.Add(new LoginAccepted(bytes));
                    break;
                case 'J':
                    output.Add(new LoginRejected(bytes));
                    break;
                case 'S':
                    output.Add(new SequencedData(bytes, true));
                    break;
                case 'H':
                    output.Add(new ServerHeartbeat(bytes));
                    break;
                case 'Z':
                    output.Add(new EndOfSession(bytes));
                    break;
                case 'L':
                    output.Add(new LoginRequest(bytes));
                    break;
                case 'U':
                    output.Add(new UnsequencedData(bytes, true));
                    break;
                case 'R':
                    output.Add(new ClientHeartbeat(bytes));
                    break;
                case 'O':
                    output.Add(new LogoutRequest(bytes));
                    break;
                default:
                    break;
            }

        }
    }
}