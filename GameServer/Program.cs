using GameServer.Logging;
using NetworkLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTransferObjects;
using System.Threading;

namespace GameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var loggerFactory = new LoggerFactory();
            var serverManager = new ServerManager();        
            var sessionManager = new SessionManager(loggerFactory, serverManager);
            var chatManager = new ChatManager();            
            //var eventLogger = new ServerEventLogger(serverManager, loggerFactory);

            serverManager.RegisterDataCallback<ChatMessage>(chatManager.ChatMessage);

            loggerFactory.AddProvider(new ConsoleLogger());            

            serverManager.Start();

            Console.WriteLine(DateTime.Now.ToString("d") +" "+ DateTime.Now.ToString("t") + " - Server Started on port: " + serverManager.Port);
             
            while (true)
            {
                serverManager.HandleMessages();
                Thread.Sleep(10);
            }
        }

       
    }
}
