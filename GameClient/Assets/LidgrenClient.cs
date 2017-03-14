using UnityEngine;
using System.Collections;
using Lidgren.Network;
using DataTransferObjects;

public delegate void ChatEventHandler(ChatMessage message);
public delegate void RemotePlayerJoinedHandler(RemotePlayer player);
public delegate void RemotePlayerLeftHandler(RemotePlayer player);

public class LidgrenClient
{    
    private NetClient m_Client;
    private const string k_ServerAddress = "127.0.0.1";
    private const int k_Port = 12345;

    public LidgrenClient()
    {
        var config = new NetPeerConfiguration("test");
        config.ConnectionTimeout = 10;
        m_Client = new NetClient(config);
    }

    public bool Connected { get { return m_Client.ConnectionStatus == NetConnectionStatus.Connected; } }
    public long Id { get { return m_Client.UniqueIdentifier; } }

    public IEnumerator Connect()
    {
        m_Client.Start();
        var hail = m_Client.CreateMessage();
        AuthRequest id = new AuthRequest()
        {
            Name = "ClientTest"
        };

        hail.Write(new Packet(PacketType.AuthRequest, id).SerializePacket());
        m_Client.Connect(k_ServerAddress, k_Port, hail);
        Debug.Log("Connecting " + id.Name + " to " + k_ServerAddress + ":" + k_Port);

        NetConnectionStatus status = m_Client.ConnectionStatus;
        while(m_Client.ConnectionStatus != NetConnectionStatus.Disconnected)
        {
            if(m_Client.ConnectionStatus != status)
            {
                status = m_Client.ConnectionStatus;
                Debug.Log("Status: " + status);
            }
            yield return null;
        }        
    }

    public void Disconnect()
    {
        m_Client.Disconnect("on quit");
    }

    public void ReadMessages()
    {
        NetIncomingMessage message;

        while ((message = m_Client.ReadMessage()) != null)
        {
            if (message.MessageType == NetIncomingMessageType.Data)
            {
                var packet = Packet.Read(message.Data);
                switch (packet.Type)
                {
                    case PacketType.ChatMessage:
                        var cm = (ChatMessage)packet.SerializedData;
                        Debug.Log(cm.SenderId + ": " + cm.Message);
                        break;
                    case PacketType.RemotePlayer:
                        var rp = (RemotePlayer)packet.SerializedData;
                        if(rp.Connected) Debug.Log("Player " + rp.Name + " (" + rp.Id + ") has joined the game.");
                        else Debug.Log("Player " + rp.Name + " (" + rp.Id + ") has left the game.");
                        break;
                }
            }               
        }
        
    }

    public void SendMessage(byte[] data, NetDeliveryMethod method, MessageChannel channel)
    {
        var msg = m_Client.CreateMessage();
        msg.Write(data);
        m_Client.SendMessage(msg, method, (int)channel);
    }
	
   

}
