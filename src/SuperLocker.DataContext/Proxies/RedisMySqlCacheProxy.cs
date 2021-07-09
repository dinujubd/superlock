using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperLocker.DataContext.Proxies
{
    public class RedisMySqlCacheProxy : ICacheProxy
    {
        private readonly IRedisDatabase _cache;
        public RedisMySqlCacheProxy(IRedisDatabase cache)
        {
            _cache = cache;
        }
        public async Task<T> QueryWithCache<T>(string key, Func<Task<T>> query) 
        {
            var record = await _cache.GetAsync<T>(key);

            if (record == null)
            {
                record = await query.Invoke();

                await _cache.AddAsync<T>(key, record);
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
