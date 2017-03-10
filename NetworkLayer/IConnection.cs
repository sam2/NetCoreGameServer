using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkLayer
{
    public interface IConnection
    {
        long Id { get; }
        string Ip { get; }
    }
}
