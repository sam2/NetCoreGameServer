using GameServer.Logging;
using NetworkLayer;

namespace GameServer.Services
{
    public class ServerEventLogger
    {
        private Logger<ServerEventLogger> m_Logger;

        public ServerEventLogger(IServer manager, LoggerFactory loggerFactory)
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
