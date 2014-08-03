using System;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace James.Testing.Rest
{
    internal class PostRequest<TBody, TResponse, TError> : RequestBase<TResponse, TError>
    {
        private readonly TBody _body;

        public PostRequest(string uriString, TBody body, object headers, object query)
            : base(uriString, headers, query)
        {
            _body = body;
        }

        protected override IResponse<TResponse, TError> GetResponse(Uri uri, HttpClient client)
        {
            var response = client.PostAsync(uri.PathAndQuery, _body, new JsonMediaTypeFormatter()).Result;
            return new Response<TResponse, TError>(response, r => r.Content.ReadAsAsync<TResponse>().Result);
        }
    }
}