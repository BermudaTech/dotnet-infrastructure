using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bermuda.Core.Cache
{
    public static class CacheManagerExtension
    {
        public static IServiceCollection AddCacheManager(this IServiceCollection services)
        {
            ThrowHelper.ThrowIfNull(services);
            services.AddMemoryCache();
            services.TryAdd(ServiceDescriptor.Singleton<ICacheManager, CacheManager>());
            return services;
        }
    }
}