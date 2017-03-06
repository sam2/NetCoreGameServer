using GameServer;
using Lidgren.Network;
using NetworkLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new NetPeerConfiguration("test");
            var client = new NetClient(config);
            client.Start();
            client.Connect("127.0.0.1", 12345);

            var msg = client.CreateMessage();
            var cm = new ChatMessage() { Message = "TESTING MESSAGE" };

            var serializer = new JMessage();
            msg.Write(serializer.Serialize(JMessage.FromValue<ChatMessage>(cm)));
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);

            while(true)
            { }
        }
    }
}

