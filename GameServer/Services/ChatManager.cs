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

            m_Server.OnConnected += PlayerConnected;
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

        public void PlayerConnected(IMessage message)
        {
            string name = m_SessionManager.GetPlayerContext(message.Connection.Id)?.Name;
            if(name == null)
            {
                m_Logger.Log(LogLevel.Error, "No session found for " + message.Connection.Id);
            }

            RemotePlayer p = new RemotePlayer()
            {
                Name = name,
                Id = message.Connection.Id
            };

            Packet packet = new Packet(PacketType.RemotePlayer, p);
            m_Server.SendAll(packet.SerializePacket(), null, DeliveryMethod.ReliableOrdered, (int)MessageChannel.Chat);
        }

        private void SendChatMessage(ChatMessage cm)
        {

        }
    }
}
