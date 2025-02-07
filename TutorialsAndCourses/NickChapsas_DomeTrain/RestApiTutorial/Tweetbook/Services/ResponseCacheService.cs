﻿using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Tweetbook.Services;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;

    public ResponseCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task CacheResponseAsync(string cacheKey, object? response, TimeSpan timeToLive)
    {
        if (response is null) return;

        var serializedResponse = JsonConvert.SerializeObject(response);

        await _distributedCache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeToLive
        });
    }

    public async Task<string?> GetCachedResponseOrDefaultAsync(string cacheKey)
    {
        var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
        return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
    }
}