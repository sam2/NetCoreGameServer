using Lidgren.Network;
using NetworkLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new NetPeerConfiguration("test");
            config.Port = 12345;
            config.MaximumConnections = 10;
            config.ConnectionTimeout = 10;
            var server = new NetServer(config);

            ClientManager clientManager = new ClientManager();

            ChatManager chatManager = new ChatManager();

            var serializer = new JMessage();

            DataPacketHandler handler = new DataPacketHandler(server, serializer);
            handler.RegisterCallback<ChatMessage>(chatManager.ChatMessage);

            server.Start();                   
            while (true)
            {
                
                NetIncomingMessage message;
                while ((message = server.ReadMessage()) != null)
                {
                    long id = message.SenderConnection.Peer.UniqueIdentifier;
                    switch (message.MessageType)
                    {                     
                        case NetIncomingMessageType.StatusChanged:
                            switch (message.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:
                                    clientManager.AddClient(id, new Client() { Name = "testUser" });                                    
                                    break;
                                case NetConnectionStatus.Disconnected:
                                    clientManager.RemoveClient(id);
                                    break;
                            }
                            break;
                        case NetIncomingMessageType.Data:
                            handler.TriggerCallback(message.ReadString());
                            break;
                        case NetIncomingMessageType.DebugMessage:
                            // handle debug messages
                            // (only received when compiled in DEBUG mode)
                            Console.WriteLine(message.ReadString());
                            break;

                        /* .. */
                        default:
                            Console.WriteLine("unhandled message with type: "
                                + message.MessageType);
                            break;
                    }
                }
            }
        }

       
    }
}
