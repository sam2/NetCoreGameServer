using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransferObjects
{
    public interface ISerializable
    {
        byte[] Serialize();
        void Deserialize(byte[] data);
    }

    public enum PacketType
    {
        ChatMessage
    }
}
