using System;
using System.Net.Http;

namespace James.Testing.Rest
{
    internal class DeleteRequest<TResponse, TError> : RequestBase<TResponse, TError>
    {
        public DeleteRequest(string uriString, object headers, DynamicDictionary query)
            : base(uriString, headers, query)
        {}

        protected override HttpRequestMessage GetRequestMessage(Uri uri)
        {
            return new HttpRequestMessage(HttpMethod.Delete, uri.PathAndQuery);
        }
    }
}