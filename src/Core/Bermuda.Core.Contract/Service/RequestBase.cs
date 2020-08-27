using System;

namespace Bermuda.Core.Contract.Service
{
    public class RequestBase
    {
        public RequestHeader Header { get; set; }

        public RequestBase()
        {
            Header = new RequestHeader();
        }
    }

    public class RequestHeader
    {
        public string Lang { get; set; }
        public string TrackId { get; set; }
        public DateTime RequestDate { get; set; }

        public RequestHeader()
        {
            Lang = "en";
        }
    }
}
