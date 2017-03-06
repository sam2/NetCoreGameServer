using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkLayer
{
    public interface ISerializer
    {
        object Deserialize(string data);
        string Serialize(object obj);
    }
}
