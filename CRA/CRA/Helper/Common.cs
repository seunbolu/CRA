using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace CRA.Helper
{
    public class Common
    {

        public static string XmlSerializeType(Type type, object data)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, data);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

    
        public static object XmlDeserialize(Type type, string input)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (MemoryStream stream = new MemoryStream(System.Text.UTF8Encoding.ASCII.GetBytes(input)))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return serializer.Deserialize(reader);
                }
            }

        }
    }
}