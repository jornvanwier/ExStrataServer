using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using vtortola.WebSockets;


namespace ExStrataServer.Communication.Server
{
    public class AsyncSocketListener
    {
        private const int port = 912;
        private static CancellationToken cancellationToken = new CancellationToken();
        private static WebSocketListener server;

        public static void Initialize()
        {
            IPAddress ip = Array.Find(Dns.GetHostEntry(Dns.GetHostName()).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);

            server = new WebSocketListener(new IPEndPoint(ip, port));
            vtortola.WebSockets.Rfc6455.WebSocketFactoryRfc6455 rfc6455 = new vtortola.WebSockets.Rfc6455.WebSocketFactoryRfc6455(server);
            server.Standards.RegisterStandard(rfc6455);
            server.Start();

            Log.Message(String.Format("Server initialized at {0}:{1}", ip, port));

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
                    string result;

                    using (StreamReader reader = new StreamReader(messageStream))
                    {
                        result = reader.ReadToEnd();
                    }
                    string response = MessageParser.Parse(result);

                    SendResponse(response, client);
                }

            }
        }

        private static async void SendResponse(string response, WebSocket client)
        {
            await client.WriteStringAsync(response, cancellationToken);
        }
    }
}
