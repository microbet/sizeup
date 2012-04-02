using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace SizeUp.Core.Serialization
{
    public static class Serializer
    {
        public static string ToJSON(object obj)
        {
            /*NOTE: if your object doesnt have getters and setters then the property wont render out in JSON*/
            if (obj == null)
            {
                return "null";
            }
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            return Encoding.Default.GetString(ms.ToArray());
        }

        public static string ToBase64(object obj)
        {
            /*NOTE: if your object doesnt have getters and setters then the property wont render out*/
            if (obj == null)
            {
                return string.Empty;
            }
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, obj);
            return Convert.ToBase64String(ms.ToArray());
        }

        public static object FromBase64(string base64)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] b = Convert.FromBase64String(base64);
            ms.Write(b, 0, b.Length);
            return serializer.Deserialize(ms);
        }

        public static byte[] ToBytes(object obj)
        {
            /*NOTE: if your object doesnt have getters and setters then the property wont render out*/
            if (obj == null)
            {
                return new byte[0];
            }
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static object FromBytes(byte[] bytes)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;
            return serializer.Deserialize(ms);
        }
    }
}
