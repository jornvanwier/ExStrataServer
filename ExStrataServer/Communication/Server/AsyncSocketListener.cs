using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using vtortola.WebSockets;
using vtortola.WebSockets.Rfc6455;

namespace ExStrataServer.Communication.Server
{
    public class AsyncSocketListener
    {
        private const int port = 913;
        private static CancellationToken cancellationToken = new CancellationToken();
        private static WebSocketListener server;

        public static void Initialize()
        {
            IPAddress ip = Array.Find(Dns.GetHostEntry(Dns.GetHostName()).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);

            WebSocketListenerOptions serverOptions = new WebSocketListenerOptions();
            serverOptions.UseDualStackSocket = false;
            server = new WebSocketListener(new IPEndPoint(ip, port), serverOptions);
            WebSocketFactoryRfc6455 rfc6455 = new WebSocketFactoryRfc6455(server);
            server.Standards.RegisterStandard(rfc6455);
            server.Start();

            Log.Message(String.Format("Server initialized at {0}:{1}", ip, port));

            Thread worker = new Thread(StartListening);
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
                    string response = await MessageParser.Parse(result);

                    SendResponse(response, client);
                }

            }
        }

        private static async void SendResponse(string response, WebSocket client)
        {
            try
            {
                 await client.WriteStringAsync(response, cancellationToken);
            } catch
            {
                Console.WriteLine("Connection closed.");
            }
        }
    }
}
