using Bermuda.Core.Repository.Repository;

namespace Bermuda.Core.Contract.Service
{
    public class ResponseBaseForPaging : ResponseBase
    {
        public int TotalCount { get; set; }
    }
}
