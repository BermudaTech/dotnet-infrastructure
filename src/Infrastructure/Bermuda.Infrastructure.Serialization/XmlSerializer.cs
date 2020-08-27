using Bermuda.Core.Serialization;
using System.IO;
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

        public Task<T> DeserializeFromXmlAsync<T>(string xml)
        {
            return Task.Run(() =>
            {
                return DeserializeFromXml<T>(xml);
            });
        }

        public async Task SerializeToXmlAsync<T>(T obj, string fileName)
        {
            await Task.Run(() =>
            {
                SerializeToXml<T>(obj, fileName);
            });
        }

        public async Task SerializeToXmlAsync<T>(T obj, string fileName, string nameSpace)
        {
            await Task.Run(() =>
            {
                SerializeToXml<T>(obj, fileName, nameSpace);
            });
        }
    }
}
