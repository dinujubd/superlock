using System.Collections.Generic;

namespace SuperLocker.Core
{
    public sealed class QueryResponse<TResponse>
    {
        public QueryResponse()
        {
            Errors = new List<string>();
        }
        public bool IsValid
        {
            get
            {
                return Errors.Count == 0;
            }
        }
        public List<string> Errors { get; set; }

        public TResponse Response;
    }
}