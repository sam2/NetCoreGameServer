using Lidgren.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkLayer
{
    public class DataPacketHandler
    {
        private NetServer m_Server;
        private ISerializer m_Serializer;
        private Dictionary<Type, Action<object>> m_MessageTypes;

        public DataPacketHandler(NetServer server, ISerializer serializer)
        {
            m_Server = server;
            m_Serializer = serializer;
            m_MessageTypes = new Dictionary<Type, Action<object>>();
        }

        public void RegisterCallback<T>(Action<T> callback)
        {
            m_MessageTypes.Add(typeof(T), cb => callback((T)cb));
        }

        public void TriggerCallback(string data)
        {
            var obj = m_Serializer.Deserialize(data);
            m_MessageTypes[obj.GetType()](obj);
        }
    }
}
