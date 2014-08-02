using System;
using System.Net.Http;

namespace James.Testing.Rest
{
    internal class GetAsDynamic<TError> : RequestBase<object, TError>
    {
        public GetAsDynamic(string uriString, object headers, object query)
            : base(uriString, headers, query)
        {}

        protected override IResponse<object, TError> GetResponse(Uri uri, HttpClient client)
        {
            var response = client.GetAsync(uri.PathAndQuery).Result;
            return new Response<object, TError>(response, r => r.Content.ReadAsAsync<object>().Result);
        }
    }
}