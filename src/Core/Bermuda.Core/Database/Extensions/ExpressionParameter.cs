using Bermuda.Core.Repository.Enum;

namespace Bermuda.Core.Database.Extensions
{
    public class ExpressionParameter
    {
        public string Value { get; set; }
        public string Property { get; set; }
        public OperatorType Operator { get; set; }
    }
}
