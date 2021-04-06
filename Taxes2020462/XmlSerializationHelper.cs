using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DividendTaxCalculatorLib.Common
{
    public static class XmlSerializationHelper
    {
        /// <summary>
        /// Deserializes object from the xml using XmlSerializer. 
        /// </summary>
        public static T Deserialize<T>(string xml) where T : class
        {
            if (xml == null)
                return null;

            using (StringReader reader = new StringReader(xml))
            {
                try
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        string.Format("Unable to deserialize {0} from xml.", typeof(T).Name), ex);
                }
            }
        }

        public static T Deserialize<T>(Stream stream) where T : class
        {
            if (stream == null)
                return null;

            try
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to deserialize {0} from xml stream.", typeof(T).Name), ex);
            }
        }

        public static string Serialize<T>(T value) where T : class
        {
            if (value == null)
                return null;

            using (StringWriter writer = new StringWriter())
            {
                try
                {
                    new XmlSerializer(typeof(T)).Serialize(writer, value);

                    return writer.ToString();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        string.Format("Unable to serialize {0} to xml.", typeof(T).Name), ex);
                }
            }
        }
    }
}
