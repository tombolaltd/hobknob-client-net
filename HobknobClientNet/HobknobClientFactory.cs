using System;
using HobknobClientNet.Api;

namespace HobknobClientNet
{
    public interface IHobknobClientFactory
    {
        IHobknobClient Create(string host, int port, string applicationName, TimeSpan cacheUpdateInterval, EventHandler<CacheUpdateFailedArgs> cacheUpdateFailed);
    }

    public class HobknobClientFactory : IHobknobClientFactory
    {
        public IHobknobClient Create(string host, int port, string applicationName, TimeSpan cacheUpdateInterval, EventHandler<CacheUpdateFailedArgs> cacheUpdateFailed)
        {
            var applicationFeaturesPath = $"http://{host}:{port}/api/applications/{applicationName}";

            var apiClient = new ApiClient(new Uri(applicationFeaturesPath));
            var featureToggleProvider = new FeatureToggleProvider(apiClient);
            var featureToggleCache = new FeatureToggleCache(featureToggleProvider, cacheUpdateInterval);
            var hobknobClient = new HobknobClient(featureToggleCache, applicationName);
            if (cacheUpdateFailed == null)
                throw new ArgumentNullException("cacheUpdateFailed", "Cached update handler is emtpy");
            featureToggleCache.CacheUpdateFailed += cacheUpdateFailed;

            featureToggleCache.Initialize();

            return hobknobClient;
        }
    }
}
