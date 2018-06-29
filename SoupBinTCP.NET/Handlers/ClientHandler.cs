using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET.Handlers
{
    internal class ClientHandler : MessageToMessageEncoder<Message>
    {
        private readonly IClientListener _listener;
        private readonly LoginDetails _loginDetails;

        public ClientHandler(LoginDetails loginDetails, IClientListener listener)
        {
            _listener = listener;
            _loginDetails = loginDetails;
        }
        
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            switch (message)
            {
                case Debug msg:
                    _listener.OnDebug(msg.Text);
                    break;
                case LoginAccepted msg:
                    _listener.OnLoginAccept(msg.Session, msg.SequenceNumber);
                    break;
                case LoginRejected msg:
                    _listener.OnLoginReject(msg.RejectReasonCode);
                    break;
                case UnsequencedData msg:
                    _listener.OnMessage(msg.Bytes);
                    break;
            }
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            _listener.OnConnect();
            context.WriteAndFlushAsync(new LoginRequest(_loginDetails.Username, _loginDetails.Password,
                _loginDetails.RequestedSession, _loginDetails.RequestedSequenceNumber));
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            _listener.OnDisconnect();
        }

        protected override void Encode(IChannelHandlerContext context, Message message, List<object> output)
        {
            var msg = Unpooled.WrappedBuffer(message.Bytes);
            output.Add(msg);
        }
    }
}