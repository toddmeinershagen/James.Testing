using System;
using System.Net.Http;

namespace James.Testing.Rest
{
    internal class DeleteRequest<TResponse, TError> : RequestBase<TResponse, TError>
    {
        public DeleteRequest(string uriString, object headers, DynamicDictionary query)
            : base(uriString, headers, query)
        {}

        protected override IResponse<TResponse, TError> GetResponse(Uri uri, HttpClient client)
        {
            var response = client.DeleteAsync(uri.PathAndQuery).Result;
            return new Response<TResponse, TError>(response);
        }
    }
}