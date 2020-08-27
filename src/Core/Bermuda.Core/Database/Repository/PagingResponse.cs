using System.Collections.Generic;

namespace Bermuda.Core.Repository.Repository
{
    public class PagingResponse<TModel> where TModel : class
    {
        public int TotalCount { get; set; }
        public IEnumerable<TModel> Result { get; set; }
    }
}
