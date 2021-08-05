using Bermuda.Core.HttpClient;
using Bermuda.Infrastructure.HttpClient.Flurl;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bermuda.Infrastructure.HttpClient.Test
{
    public class HttpClientService
    {
        private readonly IHttpClientService httpClientService;

        public HttpClientService()
        {
            this.httpClientService = new FlurlHttpClientService();
        }

        [Fact]
        public async Task GetJsonAsync()
        {
            var result = await httpClientService.GetJsonAsync<dynamic>("https://jsonplaceholder.typicode.com/todos/1");
        }
    }
}
