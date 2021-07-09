using System;
using System.Threading.Tasks;

namespace SuperLocker.DataContext.Proxies
{
    public interface ICacheProxy
    {
        Task<T> QueryWithCache<T>(string key, Func<Task<T>> query);
        Task CommandWithCacheInvalidation<T>(string key, Action command);
    }
}
