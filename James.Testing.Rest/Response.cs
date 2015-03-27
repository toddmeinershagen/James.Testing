using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace James.Testing.Rest
{
    internal class Response<TResponse, TError> : IResponse<TResponse, TError>
    {
        public TResponse Body { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public HttpResponseHeaders Headers { get; private set; }
        public TError Error { get; private set; }
        public TimeSpan ExecutionTime { get; set; }
	    public HttpContentHeaders ContentHeaders { get; set; }
	    private IEnumerable<MediaTypeFormatter> Formatters { get; set; }

        public Response(HttpResponseMessage response)
            : this(response, null)
        {} 

        public Response(HttpResponseMessage response, MediaTypeFormatter formatter)
            : this (response, formatter, TimeSpan.Zero)
        { }

        public Response(HttpResponseMessage response, MediaTypeFormatter formatter, TimeSpan executionTime)
        {
            Formatters = new[] {formatter ?? new JsonMediaTypeFormatter()};
            StatusCode = response.StatusCode;
            Headers = response.Headers;
            ExecutionTime = executionTime;
	        ContentHeaders = response.Content.Headers;

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
           
            return content.ReadAsAsync<T>(Formatters).Result;
        }
    }
}
