using NetworkLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer
{
    public class ChatManager
    {     
        public void ChatMessage(ChatMessage c)
        {
            Console.WriteLine("chat message: "+c.Message);
        }
    }
}
