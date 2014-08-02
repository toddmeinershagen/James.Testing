using System;
using System.Net.Http;

namespace James.Testing.Rest
{
    internal class GetAsBytesRequest<TError> : BaseRequest<byte[], TError>
    {
        public GetAsBytesRequest(string uriString, object headers) 
            : base(uriString, headers)
        {}

        protected override IResponse<byte[], TError> GetResponse(Uri uri, HttpClient client)
        {
            var response = client.GetAsync(uri.PathAndQuery).Result;
            return new Response<byte[], TError>(response, r => r.Content.ReadAsByteArrayAsync().Result);
        }
    }
}