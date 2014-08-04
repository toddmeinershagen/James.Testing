using System;
using System.Net.Http;

namespace James.Testing.Rest
{
    internal class DeleteRequest<TError> : RequestBase<string, TError>
    {
        public DeleteRequest(string uriString, object headers, DynamicDictionary query)
            : base(uriString, headers, query)
        {}

        protected override IResponse<string, TError> GetResponse(Uri uri, HttpClient client)
        {
            var response = client.DeleteAsync(uri.PathAndQuery).Result;
            return new Response<string, TError>(response, r => String.Empty);
        }
    }
}