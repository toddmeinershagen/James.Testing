using System;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace James.Testing.Rest
{
    internal class GetRequest<TResponse, TError> : RequestBase<TResponse, TError>
    {
        private readonly MediaTypeFormatter _formatter;

        public GetRequest(string uriString, object headers, DynamicDictionary query, MediaTypeFormatter formatter)
            : base(uriString, headers, query)
        {
            _formatter = formatter;
        }

        protected override IResponse<TResponse, TError> GetResponse(Uri uri, HttpClient client)
        {
            var response = client.GetAsync(uri.PathAndQuery).Result;
            return new Response<TResponse, TError>(response, _formatter);
        }
    }
}