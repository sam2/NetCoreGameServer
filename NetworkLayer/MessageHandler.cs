using Lidgren.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkLayer
{
    public class MessageHandler
    {
        private NetServer m_Server;
        private Dictionary<Type, Action<object>> m_MessageTypes;

        public MessageHandler(NetServer server)
        {
            m_Server = server;
            m_MessageTypes = new Dictionary<Type, Action<object>>();
        }

        public void RegisterType<T>(Action<T> callback)
        {
            m_MessageTypes.Add(typeof(T), cb => callback((T)cb));
        }

        public void Listen()
        {
            NetIncomingMessage message;
            while ((message = m_Server.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        Console.WriteLine("data");
                        JMessage msg = JMessage.Deserialize(message.ReadString());
                        var obj = JsonConvert.DeserializeObject(msg.Value.ToString(), msg.Type);
                        m_MessageTypes[msg.Type](obj);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        Console.WriteLine(message.SenderConnection.Status);
                        switch (message.SenderConnection.Status)
                        {
                            /* .. */
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine(message.ReadString());
                        break;

                    /* .. */
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + message.MessageType);
                        break;
                }
            }
        }
    }
}
