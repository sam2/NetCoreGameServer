using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkLayer
{
    public delegate void ConnectedEventHandler(NetIncomingMessage message);
    public delegate void DisonnectedEventHandler(NetIncomingMessage message);
    public delegate void ReceivedMessageEventHandler(NetIncomingMessage message);

    public class ServerManager
    {
        private NetServer m_Server;
        private ISerializer m_Serializer;
        private Dictionary<Type, Action<object>> m_MessageTypes;


        public event ConnectedEventHandler OnConnected;
        public event DisonnectedEventHandler OnDisconnected;
        public event ReceivedMessageEventHandler OnReceivedData;

        public ServerManager(ISerializer serializer)
        {
            var config = new NetPeerConfiguration("test");
            config.Port = 12345;
            config.MaximumConnections = 10;
            config.ConnectionTimeout = 10;

            m_Server = new NetServer(config);
            m_Serializer = serializer;
            m_MessageTypes = new Dictionary<Type, Action<object>>();           
        }

        public int Port { get { return m_Server.Port; } }

        public void Start()
        {
            m_Server.Start();
        }

        public void HandleMessages()
        {
            NetIncomingMessage message;
            while ((message = m_Server.ReadMessage()) != null)
            {
                long id = message.SenderConnection.Peer.UniqueIdentifier;
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        switch (message.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:
                                OnConnected?.Invoke(message);
                                break;
                            case NetConnectionStatus.Disconnected:
                                OnDisconnected?.Invoke(message);
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        TriggerCallback(message.ReadString());
                        OnReceivedData?.Invoke(message);
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

        public void RegisterDataCallback<T>(Action<T> callback)
        {
            m_MessageTypes.Add(typeof(T), cb => callback((T)cb));
        }

        public void TriggerCallback(string data)
        {
            var obj = m_Serializer.Deserialize(data);
            m_MessageTypes[obj.GetType()](obj);
        }
    }
}
