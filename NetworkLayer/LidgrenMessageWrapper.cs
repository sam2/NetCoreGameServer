using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkLayer
{
    public class LidgrenMessageWrapper : IMessage
    {
        private NetIncomingMessage m_Message;

        public LidgrenMessageWrapper(NetIncomingMessage message)
        {
            m_Message = message;
        }

        public string Description()
        {
            return m_Message.MessageType + " - " + m_Message.ReadString();
        }
    }
}
