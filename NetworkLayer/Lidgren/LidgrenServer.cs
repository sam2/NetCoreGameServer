using DataTransferObjects;
using Lidgren.Network;
using NetworkLayer.Lidgren;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkLayer.Lidgren
{   

    public class LidgrenServer : IServer
    {
        private NetServer m_Server;
        private Dictionary<Type, Action<IConnection, object>> m_MessageTypes;

        private Dictionary<DeliveryMethod, NetDeliveryMethod> m_DeliveryMethodMap = new Dictionary<DeliveryMethod, NetDeliveryMethod>()
        {
            { DeliveryMethod.Unreliable, NetDeliveryMethod.Unreliable },
            { DeliveryMethod.UnreliableSequenced, NetDeliveryMethod.UnreliableSequenced },
            { DeliveryMethod.ReliableUnordered, NetDeliveryMethod.ReliableUnordered },
            { DeliveryMethod.ReliableSequenced, NetDeliveryMethod.ReliableSequenced },
            { DeliveryMethod.ReliableOrdered, NetDeliveryMethod.ReliableOrdered }    
        };

        public event MessageEventHandler OnConnectionApproved;
        public event MessageEventHandler OnConnectionDenied;
        public event MessageEventHandler OnConnected;
        public event MessageEventHandler OnDisconnected;
        public event MessageEventHandler OnDisconnecting;
        public event MessageEventHandler OnReceivedData;

        public LidgrenServer()
        {
            var config = new NetPeerConfiguration("test");
            config.Port = 12345;
            config.MaximumConnections = 10;
            config.ConnectionTimeout = 10;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            m_Server = new NetServer(config);
            m_Server.Configuration.ConnectionTimeout = 10;

            m_MessageTypes = new Dictionary<Type, Action<IConnection, object>>();           
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
                        try
                        {
                            var packet = Packet.Read(message.Data);
                            ApproveConnection(message.SenderConnection);
                            OnConnectionApproved?.Invoke(new LidgrenMessage(message));
                        }
                        catch (Exception e)
                        {
                            DenyConnection(message.SenderConnection);
                            OnConnectionDenied?.Invoke(new LidgrenMessage(message));
                        }                  
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        switch (message.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:                               
                                OnConnected?.Invoke(new LidgrenMessage(message));                           
                                break;
                            case NetConnectionStatus.Disconnected:
                                OnDisconnecting?.Invoke(new LidgrenMessage(message)); //not firing from lidgren
                                OnDisconnected?.Invoke(new LidgrenMessage(message));
                                break;
                          
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        TriggerCallback(new LidgrenConnection(message.SenderConnection), message.Data);
                        OnReceivedData?.Invoke(new LidgrenMessage(message));
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine("DEBUG: "+message.ReadString());
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine("WARNING: "+message.ReadString());
                        break;
                    /* .. */
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + message.MessageType);
                        break;
                }                
            }
        }

        public void RegisterDataCallback<T>(Action<IConnection, T> callback)
        {
            m_MessageTypes.Add(typeof(T), (connection, cb) => callback(connection, (T)cb));
        }     

        public void SendAll(byte[] message, IConnection except, DeliveryMethod method, int channel)
        {
            NetConnection sender = null;
            if (except != null)
            {
                sender = m_Server.Connections.FirstOrDefault(x => x.RemoteUniqueIdentifier == except.Id);

                if (sender == null)
                {
                    throw new ArgumentException("Invalid except connection with id " + except.Id);               }
            }
            

            NetOutgoingMessage msg = m_Server.CreateMessage();
            msg.Write(message);
            m_Server.SendToAll(msg, sender, m_DeliveryMethodMap[method], channel);
        }

        public void Send(IConnection connection, byte[] message, DeliveryMethod method, int channel)
        {
            NetConnection recipient = m_Server.Connections.FirstOrDefault(x => x.RemoteUniqueIdentifier == connection.Id);

            if(recipient == null)
            {
                throw new ArgumentException("Could not send message to connectionId " + connection.Id + ", connection does not exist.");
            }

            NetOutgoingMessage msg = m_Server.CreateMessage();
            msg.Write(message);
            m_Server.SendMessage(msg, recipient, m_DeliveryMethodMap[method], channel);
        }

        private void TriggerCallback(IConnection connection, byte[] data)
        {
            var dto = Packet.Read(data);
            var obj = dto.SerializedData;

            m_MessageTypes[obj.GetType()](connection, obj);
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
