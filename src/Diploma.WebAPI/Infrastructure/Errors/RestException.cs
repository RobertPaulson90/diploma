using System;
using System.Net;
using System.Runtime.Serialization;

namespace Diploma.WebAPI.Infrastructure.Errors
{
    [Serializable]
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, string message = null)
            : base(message)
        {
            Code = code;
        }

        protected RestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public HttpStatusCode Code { get; }
    }
}
