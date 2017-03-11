using GameServer.Logging;
using NetworkLayer;
using System;
using DataTransferObjects;
using System.Threading;
using NetworkLayer.Lidgren;
using GameServer.Services;

namespace GameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IServer server = new LidgrenServer();

            var loggerFactory = new LoggerFactory();                
            var sessionManager = new SessionManager(loggerFactory, server);
            var chatManager = new ChatManager();            
            //var eventLogger = new ServerEventLogger(serverManager, loggerFactory);

            server.RegisterDataCallback<ChatMessage>(chatManager.ChatMessage);

            loggerFactory.AddProvider(new ConsoleLogger());            

            server.Start();

            Console.WriteLine(DateTime.Now.ToString("d") +" "+ DateTime.Now.ToString("t") + " - Server Started on port: " + server.Port);
             
            while (true)
            {
                server.HandleMessages();
                Thread.Sleep(10);
            }
        }

       
    }
}
