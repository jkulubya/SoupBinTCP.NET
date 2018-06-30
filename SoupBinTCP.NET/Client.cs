using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using SoupBinTCP.NET.Codecs;
using SoupBinTCP.NET.Handlers;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET
{
    public class Client
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private IChannel _clientChannel;
        private readonly IClientListener _listener;
        private readonly LoginDetails _loginDetails;

        public Client(IPAddress ipAddress, int port, LoginDetails loginDetails, IClientListener listener)
        {
            if(port < 0) throw new ArgumentException("Invalid port number", nameof(port));
            _ipAddress = ipAddress;
            _port = port;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _listener = listener;
            _loginDetails = loginDetails;
        }
        
        public void Start()
        {
            Task.Run(RunClientAsync);
        }
        
        public async Task Send(byte[] message)
        {
            if (message.Length > ushort.MaxValue - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(message), "SoupBinTCP message payload exceeds maximum size (65534 bytes)");
            }
            if (_clientChannel.Active)
            {
                await _clientChannel.WriteAndFlushAsync(new UnsequencedData(message));
            }
        }

        public async Task Debug(string message)
        {
            if (_clientChannel.Active)
            {
                await _clientChannel.WriteAndFlushAsync(new Debug(message));
            }
        }
        
        private async Task RunClientAsync()
        {
            var group = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;
                        pipeline.AddLast(new LengthFieldBasedFrameDecoder(ByteOrder.BigEndian, ushort.MaxValue, 0, 2,
                            0, 2, true));
                        pipeline.AddLast(new LengthFieldPrepender(ByteOrder.BigEndian, 2, 0, false));
                        pipeline.AddLast(new SoupBinTcpMessageDecoder());
                        pipeline.AddLast(new SoupBinTcpMessageEncoder());
                        pipeline.AddLast(new IdleStateHandler(15, 1, 0));
                        pipeline.AddLast(new ClientTimeoutHandler());
                        pipeline.AddLast(new ClientHandler(_loginDetails, _listener));
                    }));
                
                _clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(_ipAddress, _port));

                _cancellationToken.WaitHandle.WaitOne();

                if (_clientChannel.Active)
                {
                    await _clientChannel.WriteAndFlushAsync(new LogoutRequest());
                    await _clientChannel.CloseAsync();
                    await _listener.OnDisconnect();
                }
            }
            finally
            {
                await group.ShutdownGracefullyAsync();
            }
        }

        public void Shutdown()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}