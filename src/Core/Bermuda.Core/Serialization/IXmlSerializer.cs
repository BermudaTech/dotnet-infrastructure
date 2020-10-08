using System.Threading.Tasks;

namespace Bermuda.Core.Serialization
{
    public interface IXmlSerializer
    {
        T DeserializeFromXml<T>(string xml);

        string SerializeToXml<T>(T obj);

        void SerializeToXml<T>(T obj, string fileName);

        void SerializeToXml<T>(T obj, string fileName, string nameSpace);
    }
}
