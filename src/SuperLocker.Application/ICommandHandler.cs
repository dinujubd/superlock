using MassTransit;

namespace SuperLocker.Application
{
    public interface ICommandHandler<T> : IConsumer<T> where T : class
    {
    }
}