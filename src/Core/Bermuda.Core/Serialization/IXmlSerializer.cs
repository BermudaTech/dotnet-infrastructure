using System.Text;

namespace Bermuda.Core.Serialization
{
    public interface IXmlSerializer
    {
        T DeserializeFromXml<T>(string xml);
        string SerializeToXml<T>(T obj);
        string SerializeToXml<T>(T obj, Encoding encoding);
        void SerializeToXml<T>(T obj, string fileName);
        void SerializeToXml<T>(T obj, string fileName, string nameSpace);
    }
}
