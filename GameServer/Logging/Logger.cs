using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Logging
{
    public enum LogLevel
    {
        Fatal,
        Error,
        Warning,
        Info,
        Debug,
        Trace
    }

    class Logger<T>
    {
        public static bool IsEnabled { get; set; } = true;

        private string m_Name;
        private IEnumerable<ILoggerProvider> m_Loggers;       

        public Logger(IEnumerable<ILoggerProvider> loggers)
        {
            m_Loggers = loggers;
            m_Name = typeof(T).Name;
        }              

        public void Log(LogLevel logLevel, string message)
        {
            if(!IsEnabled)
            {
                return;
            }

            foreach(ILoggerProvider logger in m_Loggers)
            {
                logger.Log(DateTime.Now.ToString("t")+" - "+m_Name + "::" + logLevel.ToString() + " - " + message);
            }
        }
    }
}
