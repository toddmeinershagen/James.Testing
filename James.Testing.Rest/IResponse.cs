using System.Net.Http.Headers;

namespace James.Testing.Rest
{
    public interface IResponse<out TResponse>
    {
        TResponse Body { get; }
        System.Net.HttpStatusCode StatusCode { get; }
        HttpResponseHeaders Headers { get; }
    }
}
