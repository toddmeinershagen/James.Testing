using System;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace James.Testing.Rest
{
    internal class PostRequest<TRequest, TResponse, TError> : BaseRequest<TResponse, TError>
    {
        private readonly TRequest _request;

        public PostRequest(string uriString, TRequest request, object headers)
            : base(uriString, headers)
        {
            _request = request;
        }

        protected override IResponse<TResponse, TError> GetResponse(Uri uri, HttpClient client)
        {
            var response = client.PostAsync(uri.PathAndQuery, _request, new JsonMediaTypeFormatter()).Result;
            return new Response<TResponse, TError>(response, r => r.Content.ReadAsAsync<TResponse>().Result);
        }
    }
}