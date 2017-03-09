using UnityEngine;

using Lidgren.Network;
using DataTransferObjects;
using Newtonsoft.Json;

public class ClientTest : MonoBehaviour {

    NetClient client;
    bool connected = false;
    // Use this for initialization

    void Awake()
    {
        var config = new NetPeerConfiguration("test");
        client = new NetClient(config);
    }

    void Start ()
    {        
        client.Start();
        var hail = client.CreateMessage();
        hail.Write("sam");
        client.Connect("127.0.0.1", 12345, hail);                 
    }
	
	// Update is called once per frame
	void Update ()
    {
        NetIncomingMessage message;
        if (!connected)
        {
            while ((message = client.ReadMessage()) != null)
            {
                Debug.Log(message.MessageType + " - " + message.ReadString());
                connected = client.ConnectionStatus == NetConnectionStatus.Connected;
            }
        }      
    }

    public void SendChatMessage(string message)
    {
        var cm = new ChatMessage() { Message = message };
        var msg = client.CreateMessage();

        var dto = new Dto()
        {
            Type = EDtoType.ChatMessage,
            Data = JsonConvert.SerializeObject(cm)
        };
        string cmjson = JsonConvert.SerializeObject(dto);
        msg.Write(cmjson);

        client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
    }
}
