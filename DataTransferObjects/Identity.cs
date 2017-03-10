using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataTransferObjects
{
    public class Identity : ISerializable
    {
        public string Name { get; set; }

        public void Deserialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
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
                    writer.Write(Name);
                }
                return m.ToArray();
            }
        }
    }
}
