using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataTransferObjects
{
    public class RemotePlayer : ISerializable
    {
        public long Id;
        public string Name;
        public bool Connected { get; set; }

        public void Deserialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    Id = reader.ReadInt64();
                    Name = reader.ReadString();
                    Connected = reader.ReadBoolean();
                }
            }
        }

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Id);
                    writer.Write(Name);
                    writer.Write(Connected);
                }
                return m.ToArray();
            }
        }
    }
}
