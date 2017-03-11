using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.DataModel
{
    public class PlayerContext
    {
        public long Id { get; }
        public string Name { get; set;}

        public PlayerContext(long id)
        {
            Id = id;
        }
    }
}
