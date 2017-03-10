using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkLayer.Lidgren
{
    public class LidgrenMessageWrapper : IMessage
    {
        private NetIncomingMessage m_Message;

        public LidgrenMessageWrapper(NetIncomingMessage message)
        {
            m_Message = message;
        }

        public IConnection Connection
        {
            get
            {
                return new LidgrenConnectionWrapper(m_Message.SenderConnection);
            }
        }

        public string Description()
        {
            return m_Message.MessageType + " - " + m_Message.ReadString();
        }

        public byte[] Data
        {
            get
            {
                return m_Message.Data;
            }
        }
        
    }
}
