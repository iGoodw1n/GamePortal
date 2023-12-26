using Microsoft.Extensions.Caching.Memory;

namespace GamePortal.Helpers;

public static class CacheOptions
{
    public static MemoryCacheEntryOptions GetOptions()
    {
        return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal);
    }
}
