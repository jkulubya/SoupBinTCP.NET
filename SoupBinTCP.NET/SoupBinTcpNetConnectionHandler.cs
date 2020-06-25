using System.Threading.Tasks;
using Bedrock.Framework.Protocols;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace SoupBinTCP.NET
{
    public class SoupBinTcpNetConnectionHandler : ConnectionHandler
    {
        private readonly ILogger _logger;

        public SoupBinTcpNetConnectionHandler(ILogger<SoupBinTcpNetConnectionHandler> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            var protocol = new SoupBinTcpNetProtocol();
            var reader = connection.CreateReader();
            var writer = connection.CreateWriter();

            while (true)
            {
                try
                {
                    var result = await reader.ReadAsync(protocol);
                    var message = result.Message;

                    if (result.IsCompleted)
                    {
                        break;
                    }

                    await writer.WriteAsync(protocol, message);
                }
                finally
                {
                    reader.Advance();
                }
            }
        }
    }
}
