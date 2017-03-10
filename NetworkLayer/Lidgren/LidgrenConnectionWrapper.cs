using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkLayer.Lidgren
{
    class LidgrenConnectionWrapper : IConnection
    {
        private NetConnection m_NetConnection;
        public LidgrenConnectionWrapper(NetConnection connection)
        {
            m_NetConnection = connection;
        }

        public long Id { get => m_NetConnection.RemoteUniqueIdentifier; }
        public string Ip { get => m_NetConnection.RemoteEndPoint.ToString();  }
    }
}
