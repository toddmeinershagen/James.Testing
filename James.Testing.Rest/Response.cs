using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace James.Testing.Rest
{
    internal class Response<TResponse, TError> : IResponse<TResponse, TError>
    {
        public TResponse Body { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public HttpResponseHeaders Headers { get; private set; }
        public TError Error { get; private set; }

        public Response(HttpResponseMessage response, Func<HttpResponseMessage, TResponse> getContent)
        {
            StatusCode = response.StatusCode;
            Headers = response.Headers;

            if (response.IsSuccessStatusCode)
            {
                Body = getContent(response);
                Error = default(TError);
            }
            else
            {
                Body = default(TResponse);

                if (typeof (TError) == typeof (string))
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Error = (TError) (result as object);
                }
                else
                {
                    Error = response.Content.ReadAsAsync<TError>().Result;
                }
            }
        }
    }

        
}
