using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bermuda.Core.Cache
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddCacheService(this IServiceCollection services, CacheTypeEnum cacheTypeEnum = CacheTypeEnum.InMemory, string connectionString = null)
        {
            ThrowHelper.ThrowIfNull(services);

            switch (cacheTypeEnum)
            {
                case CacheTypeEnum.Redis:
                    services.AddSingleton<ICacheService>(provider => new RedisCacheService(connectionString));
                    break;
                default:
                    services.AddMemoryCache();
                    services.TryAdd(ServiceDescriptor.Singleton<ICacheService, InMemoryCacheService>());
                    break;
            }

            return services;
        }
    }
}