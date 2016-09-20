using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EntityExtension
{
    public static class ExtensionSerialize
    {
        public static byte[] SerializeToByteArray(this object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            byte[] bytes;

            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);

                bytes = stream.ToArray();
            }

            return bytes;
        }

        public static object DeserializeToObject(this byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            Stream stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(stream);
        }

    }
}
