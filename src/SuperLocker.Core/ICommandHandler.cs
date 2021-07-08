using MassTransit;

namespace SuperLocker.Core
{
    public interface ICommandHandler<T> : IConsumer<T> where T : class
    {
    }
}
