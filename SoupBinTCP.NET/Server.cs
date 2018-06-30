using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Transport.Channels.Sockets;
using SoupBinTCP.NET.Codecs;
using SoupBinTCP.NET.Handlers;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET
{
    public class Server
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        public IServerListener Listener;
        private IChannel _serverChannel;
        private readonly DefaultChannelGroup
            _channelGroup = new DefaultChannelGroup("ALL", new SingleThreadEventLoop());
        private readonly Dictionary<string, IChannel> _channels = new Dictionary<string, IChannel>();

        public Server()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
        
        public Server(IServerListener listener)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            Listener = listener;
        }

        public void Start()
        {
            Task.Run(RunServerAsync);
        }

        public async Task Send(byte[] message, string channelId)
        {
            if (message.Length > ushort.MaxValue - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(message), "SoupBinTCP message payload exceeds maximum size (65534 bytes)");
            }
            if (_channels.TryGetValue(channelId, out var channel))
            {
                await channel.WriteAndFlushAsync(new SequencedData(message));
            }
            //else throw error?
        }

        public async Task Debug(string message, string channelId)
        {
            if (_channels.TryGetValue(channelId, out var channel))
            {
                await channel.WriteAndFlushAsync(new Debug(message));
            }
            //else throw error
        }
        
        private async Task RunServerAsync()
        {
            var bossGroup =  new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;
                        _channelGroup.Add(channel);
                        _channels.Add(channel.Id.AsLongText(), channel);
                        pipeline.AddLast(new LengthFieldBasedFrameDecoder(ByteOrder.BigEndian, ushort.MaxValue, 0, 2, 0,
                            2, true));
                        pipeline.AddLast(new LengthFieldPrepender(ByteOrder.BigEndian, 2, 0, false));
                        pipeline.AddLast(new SoupBinTcpMessageDecoder());
                        pipeline.AddLast(new SoupBinTcpMessageEncoder());
                        pipeline.AddLast("LoginRequestFilter", new LoginRequestFilterHandler());
                        pipeline.AddLast(new IdleStateHandler(15, 1, 0));
                        pipeline.AddLast(new ServerTimeoutHandler(Listener));
                        pipeline.AddLast("ServerHandshake", new ServerHandshakeHandler(Listener));
                    }));

                _serverChannel = await bootstrap.BindAsync(5500);
                await Listener.OnServerListening();
                _cancellationToken.WaitHandle.WaitOne();
                if (_serverChannel.Active)
                {
                    await _channelGroup.WriteAndFlushAsync(new LogoutRequest());
                    await _serverChannel.CloseAsync();
                }

                await Listener.OnServerDisconnect();
            }
            finally
            {
                await Task.WhenAll(
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }

        public void Shutdown()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
