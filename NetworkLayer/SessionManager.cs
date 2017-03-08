using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkLayer
{
    //TODO: decouple Lidgren - replace NetConnection with IConnection
    public class SessionManager<T>
    {
        public IReadOnlyDictionary<NetConnection, T> Sessions { get { return m_Sessions; } }

        private Dictionary<NetConnection, T> m_Sessions = new Dictionary<NetConnection, T>();

        public void CreateSession(NetConnection connection, T player)
        {
            m_Sessions.Add(connection, player);
        }

        public void EndSession(NetConnection connection)
        {
            m_Sessions.Remove(connection);
        }

    }
}
