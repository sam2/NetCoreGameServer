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
            var server = new NetServer(config);

            ChatManager chatManager = new ChatManager();

            MessageHandler handler = new MessageHandler(server);
            handler.RegisterType<ChatMessage>(chatManager.ChatMessage);

            server.Start();                   

            while (true)
            {
                handler.Listen();
            }
        }

       
    }
}
