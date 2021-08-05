using Bermuda.Core.HttpClient;
using Flurl.Http;
using System.Threading.Tasks;

namespace Bermuda.Infrastructure.HttpClient.Flurl
{
    public class FlurlHttpClientService : IHttpClientService
    {
        public async Task<TResponse> PostJsonAsync<TRquest, TResponse>(TRquest request, string url)
        {
            return await url.PostJsonAsync(request)
                            .ReceiveJson<TResponse>();
        }

        public async Task<TResponse> PostJsonWithBasicAuthAsync<TRquest, TResponse>(TRquest request, string url, string username, string password)
        {
            return await url.WithBasicAuth(username, password)
                            .PostJsonAsync(request)
                            .ReceiveJson<TResponse>();
        }
    }
}
