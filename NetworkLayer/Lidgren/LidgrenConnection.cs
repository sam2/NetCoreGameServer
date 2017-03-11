using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkLayer.Lidgren
{
    class LidgrenConnection : IConnection
    {
        private NetConnection m_NetConnection;
        public LidgrenConnection(NetConnection connection)
        {
            m_NetConnection = connection;
        }

        public long Id { get => m_NetConnection.RemoteUniqueIdentifier; }
        public string Ip { get => m_NetConnection.RemoteEndPoint.ToString();  }
    }
}
