using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperLocker.DataContext.Adapters
{
    public class MySqlCacheAdapter : ICacheAdapter
    {
        private readonly IRedisDatabase _cache;
        public MySqlCacheAdapter(IRedisDatabase cache)
        {
            _cache = cache;
        }
        public async Task<T> QueryWithCache<T>(string key, Func<Task<T>> query) 
        {
            var record = await _cache.GetAsync<T>(key);

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

        public async Task PushFixed<T>(string key, T data, int MAX = 20) where T : class
        {
            var record = await _cache.GetAsync<Queue<T>>(key);

            if (record == null)
            {
                await _cache.AddAsync(key, new List<T> { data });
                return;
            }

            if(record.Count >= MAX)
            {
                record.Dequeue();
            }

            record.Enqueue(data);

            await _cache.AddAsync(key, record);
        }

    }
}
