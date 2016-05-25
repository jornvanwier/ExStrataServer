using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using vtortola.WebSockets;

namespace ExStrataServer.Communication.Server
{
    public class AsyncSocketListener
    {
        private const int port = 912;
        private static CancellationToken cancellationToken;
        private static WebSocketListener server;

        public static void Initialize()
        {
            server = new WebSocketListener(new IPEndPoint(IPAddress.Any, port));
            vtortola.WebSockets.Rfc6455.WebSocketFactoryRfc6455 rfc6455 = new vtortola.WebSockets.Rfc6455.WebSocketFactoryRfc6455(server);
            server.Standards.RegisterStandard(rfc6455);
            server.Start();

            Thread worker = new Thread(new ThreadStart(StartListening));
            worker.Start();
        }

        private static async void StartListening()
        {
            for (;;)
            {
                WebSocket client = await server.AcceptWebSocketAsync(cancellationToken);

                WebSocketMessageReadStream messageStream = await client.ReadMessageAsync(cancellationToken);

                if (messageStream != null && messageStream.MessageType == WebSocketMessageType.Text)
                {
                    using (StreamReader reader = new StreamReader(messageStream))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }

                await client.WriteStringAsync("sok", cancellationToken);
            }
        }
    }
}
