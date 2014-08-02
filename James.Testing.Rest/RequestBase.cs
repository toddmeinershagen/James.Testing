using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace James.Testing.Rest
{
    internal abstract class RequestBase<TResponse, TError> : IRequest<TResponse, TError>
    {
        private readonly string _uriString;
        private readonly object _headers;
        private readonly object _query;

        protected RequestBase(string uriString, object headers)
            : this(uriString, headers, null)
        {}

        protected RequestBase(string uriString, object headers, object query)
        {
            _uriString = uriString;
            _headers = headers;
            _query = query;
        }

        public IResponse<TResponse, TError> Execute()
        {
            var uri = new Uri(_uriString);
            using (var client = GetClient(uri))
            {
                client.AddHeaders(_headers);
                return GetResponse(uri.With(_query), client);
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

        protected abstract IResponse<TResponse, TError> GetResponse(Uri uri, HttpClient client);
    }
}