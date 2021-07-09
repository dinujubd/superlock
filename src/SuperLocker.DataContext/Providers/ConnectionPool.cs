
using System;
using System.Collections.Concurrent;

namespace SuperLocker.DataContext.Providers
{
    public class ConnectionPool<T> where T : IDisposable
    {
        private readonly ConcurrentQueue<T> _connections;
        private readonly Func<T> _objectGenerator;
        private const int MAX_CONNECTIONS = 50;

        public ConnectionPool(Func<T> createConnection)
        {
            _objectGenerator = createConnection ?? throw new ArgumentNullException(nameof(createConnection));
            _connections = new ConcurrentQueue<T>();
        }

        public T Get()
        {
            if (_connections.TryDequeue(out T item))
            {
                return item;
            }

            var connectioObject = _objectGenerator();
            _connections.Enqueue(connectioObject);

            return connectioObject;
        }

        public void Return(T item)
        {
            if (_connections.Count < MAX_CONNECTIONS)
            {
                _connections.Enqueue(item);
            }
            else
            {
                item.Dispose();
            }
        }
    }

}