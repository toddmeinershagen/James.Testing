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

        public Response(HttpResponseMessage response)
        {
            StatusCode = response.StatusCode;
            Headers = response.Headers;

            if (response.IsSuccessStatusCode)
            {
                Body = GetContent<TResponse>(response.Content);
                Error = default(TError);
            }
            else
            {
                Body = default(TResponse);
                Error = GetContent<TError>(response.Content);
            }
        }

        private T GetContent<T>(HttpContent content)
        {
            if (content == null)
                return default(T);

            var requestedType = typeof (T);

            if (requestedType == typeof(string))
            {
                var result = content.ReadAsStringAsync().Result;
                return (T)(result as object);
            }

            if (requestedType == typeof (byte[]))
            {
                object result = content.ReadAsByteArrayAsync().Result;
                return (T)result;
            }
            
            if (requestedType == typeof(object))
            {
                try
                {
                    var result = content.ReadAsAsync<object>().Result;
                    return (T)result;
                }
                catch (UnsupportedMediaTypeException ex)
                {
                    var result = content.ReadAsStringAsync().Result;
                    return (T)(result as object);
                }
            }
            
            return content.ReadAsAsync<T>().Result;
        }
    }
}
