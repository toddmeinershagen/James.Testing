using System;
using System.Net.Http;

namespace James.Testing.Rest
{
    internal class GetRequest<TResponse, TError> : BaseRequest<TResponse, TError>
    {
        public GetRequest(string uriString, object headers)
            : base(uriString, headers)
        {}

        protected override IResponse<TResponse, TError> GetResponse(Uri uri, HttpClient client)
        {
            var response = client.GetAsync(uri.PathAndQuery).Result;
            return new Response<TResponse, TError>(response, r => r.Content.ReadAsAsync<TResponse>().Result);
        }
    }
}