using GameServer.Logging;
using NetworkLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer
{
    public class ServerEventLogger
    {
        private Logger<ServerEventLogger> m_Logger;

        public ServerEventLogger(ServerManager manager, LoggerFactory loggerFactory)
        {
            m_Logger = loggerFactory.GetLogger<ServerEventLogger>();

            manager.OnConnected += LogEvent;
            manager.OnConnectionApproved += LogEvent;
            manager.OnConnectionDenied += LogEvent;
            manager.OnDisconnected += LogEvent;
            manager.OnReceivedData += LogEvent;
        }

        public void LogEvent(IMessage message)
        {
            m_Logger.Log(LogLevel.Info, message.Description());
        }
    }
}
