using System;
using System.Net;

namespace Bermuda.Core
{
    public class BusinessException : Exception
    {
        public readonly HttpStatusCode httpStatusCode;

        public BusinessException()
        {
        }

        public BusinessException(string rc) : base(rc)
        {

        }

        public BusinessException(string rc, HttpStatusCode httpStatusCode = HttpStatusCode.OK) : base(rc)
        {
            this.httpStatusCode = httpStatusCode;
        }
    }
}
