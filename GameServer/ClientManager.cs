using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer
{
    public class ClientManager
    {
        public IReadOnlyDictionary<long, Client> Clients { get { return m_Clients; } }

        private Dictionary<long, Client> m_Clients;

        public ClientManager()
        {
            m_Clients = new Dictionary<long, Client>();
        }

        public void AddClient(long id, Client client)
        {
            m_Clients.Add(id, client);
            Console.WriteLine(client.Name + "has connected.");
        }

        public void RemoveClient(long id)
        {
            string name = m_Clients[id].Name;
            m_Clients.Remove(id);
            Console.WriteLine(name + "has disconnected.");
        }
    }
}
