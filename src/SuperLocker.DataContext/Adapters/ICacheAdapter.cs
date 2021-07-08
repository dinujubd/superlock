using System;
using System.Threading.Tasks;

namespace SuperLocker.DataContext.Adapters
{
    public interface ICacheAdapter
    {
        Task<T> QueryWithCache<T>(string key, Func<Task<T>> query);
        Task CommandWithCacheInvalidation<T>(string key, Action command);
        Task PushFixed<T>(string key, T data, int MAX = 20) where T : class;
    }
}
