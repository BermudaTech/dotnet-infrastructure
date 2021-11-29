using Bermuda.Core.Serialization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Bermuda.Infrastructure.Serialization
{
    public class XmlSerializer : IXmlSerializer
    {
        public T DeserializeFromXml<T>(string xml)
        {
            T result;

            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xml)))
            {
                result = (T)xmlSerializer.Deserialize(xmlTextReader);
            }

            return result;
        }

        public string SerializeToXml<T>(T obj)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true }))
            {
                xmlSerializer.Serialize(xmlWriter, obj, null);
                return writer.ToString();
            }
        }

        public string SerializeToXml<T>(T obj, Encoding encoding)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var writer = encoding == Encoding.UTF8 ? new Utf8StringWriter() : new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
            {
                Indent = true,
                Encoding = encoding
            }))
            {
                xmlSerializer.Serialize(xmlWriter, obj, null);
                return writer.ToString();
            }
        }

        public void SerializeToXml<T>(T obj, string fileName)
        {
            SerializeToXml<T>(obj, fileName, null);
        }

        public void SerializeToXml<T>(T obj, string fileName, string nameSpace)
        {
            XmlSerializerNamespaces ns = null;

            if (!string.IsNullOrEmpty(nameSpace))
            {
                ns = new XmlSerializerNamespaces();
                ns.Add("", nameSpace);
            }

            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(fileName))
            using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true }))
            {
                xmlSerializer.Serialize(xmlWriter, obj, ns);
            }
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
