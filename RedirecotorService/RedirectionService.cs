using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedirectorService.Models;
using RedirectorService.Repositories;
using System.Timers;

namespace RedirectorService
{
    public class RedirectionService : IRedirectorService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IRedirectRepository _redirectRepository;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;
        private const string REDIRECT_CACHE_KEY = "URL_REDIRECT_CACHE";
        private const double DEFAULT_EXPIRATION = 5;
        private const int DEFAULT_REFRESH_MINUTES = 1;
        public RedirectionService(ILogger<RedirectionService> logger, IConfiguration configuration)
        {
            IOptions<MemoryCacheOptions> options = new MemoryCacheOptions();

            _memoryCache = new MemoryCache(options);

            _configuration = configuration;
            string configuredRefresh = _configuration["RedirectCacheConfig:RefreshIntervalMinutes"] ?? string.Empty;
            string configuredExpiration = _configuration["RedirectCacheConfig:ExpirationMinutes"] ?? string.Empty;
            System.Timers.Timer cacheRefreshTimer = new System.Timers.Timer(TimeSpan.FromMinutes(configuredRefresh.GetDoubleFromString(DEFAULT_REFRESH_MINUTES)).TotalMilliseconds);
            cacheRefreshTimer.AutoReset = true;
            cacheRefreshTimer.Elapsed += CacheRefresh;
            cacheRefreshTimer.Start();
            _logger = logger;
            
            _cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(configuredExpiration.GetDoubleFromString(DEFAULT_EXPIRATION)));
            _redirectRepository = new RedirectRepository(_logger);
        }

        public async Task<(string, int)> CheckForRedirect(string redirectUrl)
        {
            int redirectType = -1;
            string targetUrl = redirectUrl;
            if (!_memoryCache.TryGetValue(REDIRECT_CACHE_KEY, out IEnumerable<RedirectModel> redirectModels))
            {
                redirectModels = await _redirectRepository.GetAllAsync();
                _memoryCache.Set(REDIRECT_CACHE_KEY, redirectModels, _cacheEntryOptions);
            }

            string[] paths = redirectUrl.Split('/');
            string rootSegment = paths.Length > 2 ? $"/{paths[1]}": redirectUrl;
            RedirectModel? foundRedirect = redirectModels?.FirstOrDefault(r => r.RedirectUrl.ToLowerInvariant() == rootSegment.ToLowerInvariant());
            if (foundRedirect != null)
            {
                redirectType = foundRedirect.RedirectType;
                targetUrl = foundRedirect.TargetUrl;
                if (foundRedirect.UseRelative)
                {
                    for (int i = 2; i <= paths.Length - 1; i++) {
                        targetUrl += $"/{paths[i]}";
                    }
                }
                _logger.LogInformation($"Redirect found for {redirectUrl} redirecting to {targetUrl}");
                
            }
                

            return (targetUrl, redirectType);

        }

        private async void CacheRefresh(object sender, ElapsedEventArgs e)
        {
            _logger.LogInformation($"Refresh interval has elapsed... Refreshing Cache...");
            IEnumerable<RedirectModel> redirectModels = await _redirectRepository.GetAllAsync();

            _memoryCache.Set(REDIRECT_CACHE_KEY, redirectModels, _cacheEntryOptions);
        }

    }
}
