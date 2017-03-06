using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NetworkLayer
{
    public class JMessage : ISerializer
    {
        public Type Type { get; set; }
        public JToken Value { get; set; }

        public static JMessage FromValue<T>(T value)
        {
            return new JMessage { Type = typeof(T), Value = JToken.FromObject(value) };
        } 

        public object Deserialize(string data)
        {
            JMessage msg = JToken.Parse(data).ToObject<JMessage>();
            return JsonConvert.DeserializeObject(msg.Value.ToString(), msg.Type);
        }

        public string Serialize(object obj)
        {
            return JToken.FromObject(obj).ToString();
        }
    }
}
