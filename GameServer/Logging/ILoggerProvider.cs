using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Logging
{
    public interface ILoggerProvider
    {
        void Log(string message);
    }
}
