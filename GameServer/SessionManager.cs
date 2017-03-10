using GameServer.Logging;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetworkLayer;
using DataTransferObjects;

namespace GameServer
{
    //TODO: decouple Lidgren - replace IConnection with IConnection
    public class SessionManager
    {
        public IReadOnlyDictionary<IConnection, PlayerContext> Sessions { get { return m_Sessions; } }

        private Dictionary<IConnection, PlayerContext> m_Sessions;
        private Logger<SessionManager> m_Logger;
        private ServerManager m_Server;

        public SessionManager(LoggerFactory factory, ServerManager server)
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
                var id = (Identity)Packet.Read(message.Data).SerializedData;

                PlayerContext player = new PlayerContext()
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
