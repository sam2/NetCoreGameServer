using System;
using System.IO;

namespace DataTransferObjects
{
    public class ChatMessage : ISerializable
    {
        public long SenderId;
        public string Message;

        public void Deserialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    SenderId = reader.ReadInt64();
                    Message = reader.ReadString();
                }
            }
        }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(SenderId);
                    writer.Write(Message);
                }
                return m.ToArray();
            }
        }
    }
}
