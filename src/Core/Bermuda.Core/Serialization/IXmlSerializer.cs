using System.Threading.Tasks;

namespace Bermuda.Core.Serialization
{
    public interface IXmlSerializer
    {
        T DeserializeFromXml<T>(string xml);

        void SerializeToXml<T>(T obj, string fileName);

        void SerializeToXml<T>(T obj, string fileName, string nameSpace);

        Task<T> DeserializeFromXmlAsync<T>(string xml);

        Task SerializeToXmlAsync<T>(T obj, string fileName);

        Task SerializeToXmlAsync<T>(T obj, string fileName, string nameSpace);
    }
}
