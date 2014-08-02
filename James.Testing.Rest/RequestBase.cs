using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace James.Testing.Rest
{
    internal abstract class RequestBase<TResponse, TError> : IRequest<TResponse, TError>
    {
        private readonly string _uriString;
        private readonly object _headers;

        protected RequestBase(string uriString, object headers)
        {
            _uriString = uriString;
            _headers = headers;
        }

        public IResponse<TResponse, TError> Execute()
        {
            var uri = new Uri(_uriString);
            using (var client = GetClient(uri))
            {
                AddHeaders(_headers, client);
                return GetResponse(uri, client);
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

        private void AddHeaders(object headers, HttpClient client)
        {
            if (headers == null) return;

            foreach (var property in headers.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                client.DefaultRequestHeaders.Add(property.Name.Replace("_", "-"),
                    property.GetValue(headers, null).ToString());
            }
        }

        protected abstract IResponse<TResponse, TError> GetResponse(Uri uri, HttpClient client);
    }
}