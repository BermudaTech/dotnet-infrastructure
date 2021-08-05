using System.Threading.Tasks;

namespace Bermuda.Core.HttpClient
{
    public interface IHttpClientService
    {
        Task<TResponse> PostJsonAsync<TRequest, TResponse>(TRequest request, string url);
        Task<TResponse> PostJsonWithBasicAuthAsync<TRequest, TResponse>(TRequest request, string url, string username, string password);
    }
}
