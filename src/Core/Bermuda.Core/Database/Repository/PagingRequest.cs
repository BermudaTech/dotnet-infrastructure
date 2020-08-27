using Bermuda.Core.Database.Extensions;
using Bermuda.Core.Repository.Enum;
using System.Collections.Generic;

namespace Bermuda.Core.Repository.Repository
{
    public class PagingRequest
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string OrderBy { get; set; }
        public OrderType? OrderType { get; set; }
        public List<ExpressionParameter> Expressions { get; set; }
    }
}
