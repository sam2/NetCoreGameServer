using UnityEngine;
using Lidgren.Network;
using DataTransferObjects;

public class ClientTest : MonoBehaviour {

    NetClient client;
    bool connected = false;
    // Use this for initialization

    void Awake()
    {
        var config = new NetPeerConfiguration("test");
        config.ConnectionTimeout = 10;
        client = new NetClient(config);
    }

    void Start ()
    {        
        client.Start();
        var hail = client.CreateMessage();
        Identity id = new Identity()
        {
            Name = "ClientTest"
        };

        hail.Write(new Packet(PacketType.Identity, id).SerializePacket());
        client.Connect("127.0.0.1", 12345, hail);                 
    }
	
	// Update is called once per frame
	void Update ()
    {
        connected = client.ConnectionStatus != NetConnectionStatus.Disconnected;

        NetIncomingMessage message;
        if (connected)
        {
            while ((message = client.ReadMessage()) != null)
            {
                Debug.Log(message.MessageType + " - " + message.ReadString());                
            }
        }      
    }

    public void SendChatMessage(string message)
    {
        var cm = new ChatMessage() { Message = message };
        var msg = client.CreateMessage();

        var dto = new Packet(PacketType.ChatMessage, cm);      

        msg.Write(dto.SerializePacket());

        client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
    }
}
