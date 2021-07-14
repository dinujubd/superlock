using System.Collections.Generic;

namespace SuperLocker.Application
{
    public sealed class QueryResponse<TResponse>
    {
        public TResponse Response;

        public QueryResponse()
        {
            Errors = new List<string>();
        }

        public bool IsValid => Errors.Count == 0;

        public List<string> Errors { get; set; }
    }
}