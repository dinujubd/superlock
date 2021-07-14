using System;
using System.Threading.Tasks;

namespace SuperLocker.Infrastructure.Proxies
{
    public interface ICacheProxy
    {
        Task<T> QueryWithCache<T>(string key, Func<Task<T>> query);
        Task CommandWithCacheInvalidation<T>(string key, Action command);
    }
}