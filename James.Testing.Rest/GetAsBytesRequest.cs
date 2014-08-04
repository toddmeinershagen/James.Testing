using System;
using System.Net.Http;

namespace James.Testing.Rest
{
    internal class GetAsBytes<TError> : RequestBase<byte[], TError>
    {
        public GetAsBytes(string uriString, object headers, DynamicDictionary query) 
            : base(uriString, headers, query)
        {}

        protected override IResponse<byte[], TError> GetResponse(Uri uri, HttpClient client)
        {
            var response = client.GetAsync(uri.PathAndQuery).Result;
            return new Response<byte[], TError>(response, r => r.Content.ReadAsByteArrayAsync().Result);
        }
    }
}