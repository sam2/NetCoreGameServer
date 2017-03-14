using GameServer.Logging;
using System;
using System.Collections.Generic;
using NetworkLayer;
using DataTransferObjects;
using GameServer.DataModel;

namespace GameServer.Services
{
    public class SessionManager
    {      

        private Dictionary<long, PlayerContext> m_Sessions;
        private Logger<SessionManager> m_Logger;
        private IServer m_Server;

        public SessionManager(LoggerFactory factory, IServer server)
        {
            m_Logger = factory.GetLogger<SessionManager>();
            m_Sessions = new Dictionary<long, PlayerContext>();
            m_Server = server;

            server.OnConnectionApproved += CreateSession;
            server.OnConnected += PlayerConnected;
            server.OnDisconnecting += PlayerDisconnected;
            server.OnDisconnected += EndSession;
        }

        public PlayerContext GetPlayerContext(long connectionId)
        {
            if (!m_Sessions.TryGetValue(connectionId, out PlayerContext ctx))
            {
                m_Logger.Log(LogLevel.Error, "Could not find session for " + connectionId);
            }

            return ctx;
        }

        private void CreateSession(IMessage message)
        {
            try
            {
                var id = (AuthRequest)Packet.Read(message.Data).SerializedData;

                PlayerContext player = new PlayerContext(message.Connection.Id)
                {
                    Name = id.Name
                };

                m_Sessions.Add(message.Connection.Id, player);
                m_Logger.Log(LogLevel.Info, "Created session for PeerId " + message.Connection.Id + " at " + message.Connection.Ip);
            }
            catch (Exception e)
            {
                m_Logger.Log(LogLevel.Error, e.Message);
            }
            
        }

        private void EndSession(IMessage message)
        {
            var connection = message.Connection;
            m_Logger.Log(LogLevel.Info, "Ended session for PeerId " + connection.Id + " at " + connection.Ip);
            m_Sessions.Remove(connection.Id);
        }

        private void PlayerConnected(IMessage message)
        {
            string name = GetPlayerContext(message.Connection.Id)?.Name;
            if (name == null)
            {
                m_Logger.Log(LogLevel.Error, "No session found for " + message.Connection.Id);
            }

            RemotePlayer p = new RemotePlayer()
            {
                Name = name,
                Id = message.Connection.Id,
                Connected = true
            };

            Packet packet = new Packet(PacketType.RemotePlayer, p);
            m_Server.SendAll(packet.SerializePacket(), null, DeliveryMethod.ReliableOrdered, (int)MessageChannel.Chat);
        }

        private void PlayerDisconnected(IMessage message)
        {
            string name = GetPlayerContext(message.Connection.Id)?.Name;
            if (name == null)
            {
                m_Logger.Log(LogLevel.Error, "No session found for " + message.Connection.Id);
            }

            RemotePlayer p = new RemotePlayer()
            {
                Name = name,
                Id = message.Connection.Id,
                Connected = false
            };

            Packet packet = new Packet(PacketType.RemotePlayer, p);
            m_Server.SendAll(packet.SerializePacket(), null, DeliveryMethod.ReliableOrdered, (int)MessageChannel.Chat);
        }

    }
}
