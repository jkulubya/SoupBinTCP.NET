using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Common.Internal.Logging;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging.Console;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET
{
    public class Server
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private readonly IServerListener _listener;
        private IChannel _serverChannel;
        private readonly DefaultChannelGroup
            _channelGroup = new DefaultChannelGroup("ALL", new SingleThreadEventLoop());
        private readonly Dictionary<string, IChannel> _channels = new Dictionary<string, IChannel>();

        public Server(IServerListener listener)
        {
            InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _listener = listener;
        }

        public void Start()
        {
            Task.Run(RunServerAsync);
        }

        public async Task Send(string channelId)
        {
            throw new NotImplementedException();
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
                    .Handler(new LoggingHandler("LSTN"))
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;
                        _channelGroup.Add(channel);
                        _channels.Add(channel.Id.AsLongText(), channel);
                        //pipeline.AddLast(new LoggingHandler("CONN"));
                        
                        pipeline.AddLast(new LengthFieldBasedFrameDecoder(ByteOrder.BigEndian, ushort.MaxValue, 0, 2, 0,
                            2, true));
                        pipeline.AddLast(new SoupBinTcpMessageDecoder());
                        pipeline.AddLast("LoginRequestFilter", new LoginRequestFilterHandler());
                        pipeline.AddLast(new IdleStateHandler(15, 1, 0));
                        pipeline.AddLast("ServerHandshakeTimeout", new ServerHandshakeTimeoutHandler());
                        pipeline.AddLast("ServerHandshake", new ServerHandshakeHandler(_listener));
                        //pipeline.AddLast(new ServerTimeoutHandler());
                        //pipeline.AddLast(new ServerHandler());
                    }));
                
                _serverChannel = await bootstrap.BindAsync(5500);
                _cancellationToken.WaitHandle.WaitOne();
                await _serverChannel.CloseAsync();
            }
            finally
            {
                await Task.WhenAll(
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
        }

        public async Task Shutdown()
        {
            await _channelGroup.WriteAndFlushAsync(new LogoutRequest());
            _cancellationTokenSource.Cancel();
        }
    }
}
