using Microsoft.Extensions.Caching.Distributed;
using SuperLocker.Crosscuts.Extensions;
using System;
using System.Threading.Tasks;

namespace SuperLocker.DataContext.Adapters
{
    public class MySqlCacheAdapter : ICacheAdapter
    {
        private readonly IDistributedCache _cache;
        public MySqlCacheAdapter(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<T> QueryWithCache<T>(string key, Func<Task<T>> query)
        {
            var record = await _cache.GetRecordAsync<T>(key);

            if (record == null)
            {
                record = await query.Invoke();
            }

            return record;
        }

        public async Task CommandWithCacheInvalidation<T>(string key, Action command)
        {
            await _cache.RemoveAsync(key);
            command.Invoke();
        }
    }
}
