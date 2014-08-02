using System;
using System.Net.Http;

namespace James.Testing.Rest
{
    internal class GetAsDynamicRequest<TError> : BaseRequest<object, TError>
    {
        public GetAsDynamicRequest(string uriString, object headers)
            : base(uriString, headers)
        {}

        protected override IResponse<object, TError> GetResponse(Uri uri, HttpClient client)
        {
            var response = client.GetAsync(uri.PathAndQuery).Result;
            return new Response<object, TError>(response, r => r.Content.ReadAsAsync<object>().Result);
        }
    }
}