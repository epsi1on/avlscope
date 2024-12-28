using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persiscope.Common
{
    public static class SerializationUtil
    {
        public static byte[] Serialize(object obj)
        {
            using (var str = new MemoryStream())
            {
                JsonSerializer.Serialize(str, obj);

                return str.ToArray();
            }
        }

        public static object DeSerialize(byte[] data, Type t)
        {
            using (var str = new MemoryStream(data))
            {
                var buf = JsonSerializer.Deserialize(str, t);

                return buf;
            }
        }
    }
}
