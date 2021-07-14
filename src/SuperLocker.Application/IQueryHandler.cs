using System.Threading.Tasks;

namespace SuperLocker.Application
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery
    {
        Task<QueryResponse<TResponse>> ExecuteAsync(TQuery query);
    }
}