using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperLocker.DataContext.Adapters
{
    public class RedisCacheAdapter : ICacheAdapter
    {
        private readonly IRedisDatabase _cache;
        public RedisCacheAdapter(IRedisDatabase cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// This is an utility to make a fixed size queue in redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="MAX"></param>
        /// <returns></returns>
        public async Task PushFixed<T>(string key, T data, int MAX = 20) where T : class
        {
            var record = await _cache.GetAsync<Queue<T>>(key);

            if (record == null)
            {
                await _cache.AddAsync(key, new List<T> { data });
                return;
            }

            if (record.Count >= MAX)
            {
                record.Dequeue();
            }

            record.Enqueue(data);

            await _cache.AddAsync(key, record);
        }
    }
}
