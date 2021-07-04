using System.Threading.Tasks;

namespace SuperLocker.Core
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task HandleAsync(T command);
    }
}
