using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace James.Testing.Rest
{
    internal abstract class RequestBase<TResponse, TError> : IRequest<TResponse, TError>
    {
        private readonly string _uriString;
        private readonly object _headers;
        private readonly DynamicDictionary _query;
        protected readonly MediaTypeFormatter Formatter;

        protected RequestBase(string uriString, object headers, DynamicDictionary query)
            : this (uriString, headers, query, new JsonMediaTypeFormatter())
        { }

        protected RequestBase(string uriString, object headers, DynamicDictionary query, MediaTypeFormatter formatter)
        {
            _uriString = uriString;
            _headers = headers;
            _query = query;
            Formatter = formatter;
        }

        public IResponse<TResponse, TError> Execute()
        {
            var uri = new Uri(_uriString);
            using (var client = GetClient(uri))
            {
                client.AddHeaders(_headers);

                var watch = new Stopwatch();
                watch.Start();
                var response = client.SendAsync(GetRequestMessage(uri.With(_query))).Result;
                watch.Stop();

                return new Response<TResponse, TError>(response, Formatter, watch.Elapsed);
            }
        }

        private HttpClient GetClient(Uri uri)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(String.Format("{0}://{1}:{2}", uri.Scheme, uri.Host, uri.Port))
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        protected abstract HttpRequestMessage GetRequestMessage(Uri uri);
    }
}