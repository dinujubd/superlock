using System.Threading.Tasks;

namespace SuperLocker.Core
{
    public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery
    {
        Task<TResponse> ExecuteAsync(TQuery query);
    }
}
