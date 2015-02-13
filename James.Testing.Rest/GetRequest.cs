using System;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace James.Testing.Rest
{
    internal class GetRequest<TResponse, TError> : RequestBase<TResponse, TError>
    {
        public GetRequest(string uriString, object headers, DynamicDictionary query, MediaTypeFormatter formatter)
            : base(uriString, headers, query, formatter)
        {}

        protected override HttpRequestMessage GetRequestMessage(Uri uri)
        {
            return new HttpRequestMessage(HttpMethod.Get, uri.PathAndQuery);
        }
    }
}