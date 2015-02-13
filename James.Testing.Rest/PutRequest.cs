using System;
using System.Net.Http;

namespace James.Testing.Rest
{
    internal class PutRequest<TBody, TResponse, TError> : RequestBase<TResponse, TError>
    {
        private readonly TBody _body;

        public PutRequest(string uriString, TBody body, object headers, DynamicDictionary query)
            : base(uriString, headers, query)
        {
            _body = body;
        }

        protected override HttpRequestMessage GetRequestMessage(Uri uri)
        {
            return new HttpRequestMessage(HttpMethod.Put, uri.PathAndQuery)
            {
                Content = new ObjectContent<TBody>(_body, Formatter)
            };
        }
    }
}