
using System;
using System.Collections.Concurrent;

namespace SuperLocker.DataContext.Providers
{
    public class ConnectionPool<T> where T: IDisposable
    {
        private readonly ConcurrentQueue<T> _objects;
        private readonly Func<T> _objectGenerator;
        private const int MAX_CONNECTIONS = 50;

        public ConnectionPool(Func<T> objectGenerator)
        {
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
            _objects = new ConcurrentQueue<T>();
        }

        public T Get()
        {
            if (_objects.TryDequeue(out T item))
            {
                return item;
            }

            var connectioObject = _objectGenerator();
            _objects.Enqueue(connectioObject);

            return connectioObject;
        }

        public void Return(T item)
        {
            if (_objects.Count < MAX_CONNECTIONS)
               _objects.Enqueue(item);
            else
                item.Dispose();
        }
    }

}