using System.Threading.Tasks;

namespace SuperLocker.DataContext.Adapters
{
    public interface ICacheAdapter
    {
        Task PushFixed<T>(string key, T data, int MAX = 20) where T : class;
    }
}
