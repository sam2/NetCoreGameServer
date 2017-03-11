using GameServer.Logging;
using System;
using System.Collections.Generic;
using NetworkLayer;
using DataTransferObjects;
using GameServer.DataModel;

namespace GameServer.Services
{
    //TODO: decouple Lidgren - replace IConnection with IConnection
    public class SessionManager
    {
        public IReadOnlyDictionary<IConnection, PlayerContext> Sessions { get { return m_Sessions; } }

        private Dictionary<IConnection, PlayerContext> m_Sessions;
        private Logger<SessionManager> m_Logger;

        public SessionManager(LoggerFactory factory, IServer server)
        {
            m_Logger = factory.GetLogger<SessionManager>();
            m_Sessions = new Dictionary<IConnection, PlayerContext>();

            server.OnConnectionApproved += CreateSession;
            server.OnDisconnected += EndSession;
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

                m_Sessions.Add(message.Connection, player);
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
            m_Sessions.Remove(connection);
        }     

    }
}
