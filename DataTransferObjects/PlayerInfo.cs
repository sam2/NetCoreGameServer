using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataTransferObjects
{
    class PlayerInfo : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public void Deserialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    Id = reader.ReadInt32();
                    Name = reader.ReadString();
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
                }
                return m.ToArray();
            }
        }
    }
}
