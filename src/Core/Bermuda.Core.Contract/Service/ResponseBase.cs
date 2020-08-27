namespace Bermuda.Core.Contract.Service
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            this.IsSuccess = true;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
