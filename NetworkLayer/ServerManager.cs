using DataTransferObjects;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkLayer
{
    public delegate void MessageEventHandler(IMessage message);

    public class ServerManager
    {
        private NetServer m_Server;
        private Dictionary<Type, Action<object>> m_MessageTypes;

        public event MessageEventHandler OnConnectionApproved;
        public event MessageEventHandler OnConnectionDenied;
        public event MessageEventHandler OnConnected;
        public event MessageEventHandler OnDisconnected;
        public event MessageEventHandler OnReceivedData;

        public ServerManager()
        {
            var config = new NetPeerConfiguration("test");
            config.Port = 12345;
            config.MaximumConnections = 10;
            config.ConnectionTimeout = 10;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            m_Server = new NetServer(config);

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
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        string name = message.PeekString();
                        if (!string.IsNullOrEmpty(name))
                        {
                            ApproveConnection(message.SenderConnection);
                            OnConnectionApproved?.Invoke(new LidgrenMessageWrapper(message));                            
                        }
                        else
                        {
                            DenyConnection(message.SenderConnection);
                            OnConnectionDenied?.Invoke(new LidgrenMessageWrapper(message));  
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch (message.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:                               
                                OnConnected?.Invoke(new LidgrenMessageWrapper(message));                           
                                break;
                            case NetConnectionStatus.Disconnected:
                                OnDisconnected?.Invoke(new LidgrenMessageWrapper(message));
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        TriggerCallback(message.Data);
                        OnReceivedData?.Invoke(new LidgrenMessageWrapper(message));
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

        private void TriggerCallback(byte[] data)
        {
            var dto = Packet.Read(data);
            var obj = dto.Data;

            m_MessageTypes[obj.GetType()](obj);
        }

        private void ApproveConnection(NetConnection connection)
        {
            var msg = m_Server.CreateMessage();
            msg.Write("connected");
            connection.Approve(msg);
        }

        private void DenyConnection(NetConnection connection)
        {      
            connection.Deny("denied");
        }


    }
}
