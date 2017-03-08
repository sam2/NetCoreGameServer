using GameServer.Logging;
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
            var serializer = new JMessage();
            var serverManager = new ServerManager(serializer);        
            var sessionManager = new SessionManager<PlayerContext>();
            var chatManager = new ChatManager();
            var loggerFactory = new LoggerFactory();
            var eventLogger = new ServerEventLogger(serverManager, loggerFactory);

            serverManager.RegisterDataCallback<ChatMessage>(chatManager.ChatMessage);

            loggerFactory.AddProvider(new ConsoleLogger());            

            serverManager.Start();

            Console.WriteLine(DateTime.Now.ToString("d") +" "+ DateTime.Now.ToString("t") + " - Server Started on port: " + serverManager.Port);
             
            while (true)
            {
                serverManager.HandleMessages();
            }
        }

       
    }
}
