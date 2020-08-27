using Bermuda.Core.Repository.Repository;

namespace Bermuda.Core.Contract.Service
{
    public class RequestBaseForPaging : RequestBase
    {
        public PagingRequest Paging { get; set; }
    }
}
