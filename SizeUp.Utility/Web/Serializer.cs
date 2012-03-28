using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SizeUp.Utility.Web
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
    }
}
