using DataTransferObjects;
using GameServer.Logging;
using NetworkLayer;
using System;

namespace GameServer.Services
{
    public class ChatManager
    {
        private IServer m_Server;
        private Logger<ChatManager> m_Logger;
        private SessionManager m_SessionManager;

        public ChatManager(IServer server, LoggerFactory loggerFactory, SessionManager sessionManager)
        {
            m_Server = server;
            m_Logger = loggerFactory.GetLogger<ChatManager>();
            m_SessionManager = sessionManager;

            m_Server.RegisterDataCallback<ChatMessage>(ChatMessage);
        }

        public void ChatMessage(IConnection connection, ChatMessage c)
        {
            if(c.SenderId != connection.Id)
            {
                m_Logger.Log(LogLevel.Warning, "Sender ID " + c.SenderId + " does not match connection ID " + connection.Id);
                return;
            }
            Packet message = new Packet(PacketType.ChatMessage, c);
            m_Server.SendAll(message.SerializePacket(), null, DeliveryMethod.ReliableOrdered, (int)MessageChannel.Chat);
        }

       
    }
}
