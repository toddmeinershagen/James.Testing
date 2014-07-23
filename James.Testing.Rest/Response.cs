using System.Net;
using System.Net.Http.Headers;

namespace James.Testing.Rest
{
    internal class Response<TResponse> : IResponse<TResponse>
    {
        public Response(TResponse body, HttpStatusCode statusCode, HttpResponseHeaders headers)
        {
            Body = body;
            StatusCode = statusCode;
            Headers = headers;
        }

        public TResponse Body { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public HttpResponseHeaders Headers { get; private set; }
    }
}
