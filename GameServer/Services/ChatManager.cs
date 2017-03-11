using DataTransferObjects;
using System;

namespace GameServer.Services
{
    public class ChatManager
    {     
        public void ChatMessage(ChatMessage c)
        {
            Console.WriteLine("chat message: "+c.Message);
        }
    }
}
