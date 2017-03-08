using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Logging
{
    public class LoggerFactory
    {
        private Dictionary<string, ILoggerProvider> m_Loggers = new Dictionary<string, ILoggerProvider>();

        public void AddProvider(ILoggerProvider provider)
        {
            string providerName = provider.ToString();
            if (!m_Loggers.ContainsKey(providerName))
            {
                m_Loggers[providerName] = provider;
            }
        }

        public Logger<T> GetLogger<T>()
        {
            return new Logger<T>(m_Loggers.Values);
        }
    }
}
