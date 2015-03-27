using System;
using System.Net.Http.Headers;

namespace James.Testing.Rest
{
    public interface IResponse<out TResponse, out TError>
    {
        TResponse Body { get; }
        System.Net.HttpStatusCode StatusCode { get; }
        HttpResponseHeaders Headers { get; }
        TError Error { get; }

        TimeSpan ExecutionTime { get; }

		HttpContentHeaders ContentHeaders { get; }
    }
}
