using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataTransferObjects
{
    public class Packet
    {
        public ISerializable Data;
        private PacketType Type;

        public Packet(PacketType packetType, ISerializable data)
        {
            Type = packetType;
            Data = data;
        }

        public byte[] SerializePacket()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((byte)Type);
                    writer.Write(Data.Serialize());
                }
                return m.ToArray();
            }
        }

        public static Packet Read(byte[] packet)
        {
            PacketType p;
            byte[] data;
            using (MemoryStream m = new MemoryStream(packet))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    p = (PacketType)reader.ReadByte();
                    data = reader.ReadAllBytes();
                }
            }
            return new Packet(p, GetData(p, data));
        }

        static ISerializable GetData(PacketType packetType, byte[] data)
        {
            switch (packetType)
            {
                case PacketType.ChatMessage:
                    var ret = new ChatMessage();
                    ret.Deserialize(data);
                    return ret;
            }
            return null;
        }
    }

    public static class BinaryReaderExtensions
    {
        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }
    }
}
