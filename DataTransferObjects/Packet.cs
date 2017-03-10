using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataTransferObjects
{
    public class Packet
    {
        public ISerializable SerializedData;
        private PacketType Type;

        public Packet(PacketType packetType, ISerializable data)
        {
            Type = packetType;
            SerializedData = data;
        }

        public byte[] SerializePacket()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write((byte)Type);
                    writer.Write(SerializedData.Serialize());
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

            ISerializable sData = GetSerializedData(p, data);

            if (sData == null)
            {
                throw new ArgumentException("Invalid Packet");
            }

            return new Packet(p,sData);
        }

        static ISerializable GetSerializedData(PacketType packetType, byte[] data)
        {
            switch (packetType)
            {
                case PacketType.ChatMessage:
                    var cm = new ChatMessage();
                    cm.Deserialize(data);
                    return cm;
                case PacketType.Identity:
                    var id = new Identity();
                    id.Deserialize(data);
                    return id;
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
