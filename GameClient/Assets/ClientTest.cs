using UnityEngine;
using Lidgren.Network;
using DataTransferObjects;
using System.Collections;

public class ClientTest : MonoBehaviour {

    LidgrenClient m_Client;
    bool connected = false;
    // Use this for initialization

    void Awake()
    {
        m_Client = new LidgrenClient();
    }

    IEnumerator Start ()
    {
        yield return m_Client.Connect();  
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_Client.ReadMessages();
    }

    public void SendChatMessage(string message)
    {
        Debug.Log("sending message " + message);
        var cm = new ChatMessage() { Message = message, SenderId = m_Client.Id };

        var dto = new Packet(PacketType.ChatMessage, cm);

        m_Client.SendMessage(dto.SerializePacket(), NetDeliveryMethod.ReliableOrdered, MessageChannel.Chat);
    }

    void OnApplicationQuit()
    {
        m_Client.Disconnect();
    }
}
