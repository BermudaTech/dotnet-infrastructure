namespace Bermuda.Core.ResourceManager
{
    public interface IEmbeddedResourceReader
    {
        string ReadResourceData<TSource>(string embeddedFileName) where TSource : class;
    }
}
