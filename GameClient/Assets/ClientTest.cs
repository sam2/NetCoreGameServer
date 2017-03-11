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
        AuthRequest id = new AuthRequest()
        {
            Name = "ClientTest"
        };

        hail.Write(new Packet(PacketType.AuthRequest, id).SerializePacket());
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
                if(message.MessageType == NetIncomingMessageType.Data)
                {
                    var packet = Packet.Read(message.Data);
                    switch(packet.Type)
                    {
                        case PacketType.ChatMessage:
                            var cm = (ChatMessage)packet.SerializedData;
                            Debug.Log(cm.SenderId + ": " + cm.Message);
                            break;
                    }
                }
                Debug.Log(message.MessageType + " - " + message.ReadString());                
            }
        }      
    }

    public void SendChatMessage(string message)
    {
        var cm = new ChatMessage() { Message = message, SenderId = client.UniqueIdentifier };
        var msg = client.CreateMessage();

        var dto = new Packet(PacketType.ChatMessage, cm);      

        msg.Write(dto.SerializePacket());

        client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
    }
}
