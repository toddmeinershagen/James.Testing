using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace James.Testing.Rest
{
    public class Request
    {
        public static IResponse<TResponse> Get<TResponse>(string uriString)
        {
            var uri = new Uri(uriString);
            using (var client = GetClient(uri))
            {
                var responseTask = client.GetAsync(uri.PathAndQuery);
                var response = responseTask.Result;

                return new Response<TResponse>(response.Content.ReadAsAsync<TResponse>().Result, response.StatusCode, response.Headers);
            }
        }

        public static IResponse<byte[]> GetAsBytes(string uriString)
        {
            var uri = new Uri(uriString);
            using (var client = GetClient(uri))
            {
                var responseTask = client.GetAsync(uri.PathAndQuery);
                var response = responseTask.Result;

                return new Response<byte[]>(response.Content.ReadAsByteArrayAsync().Result, response.StatusCode, response.Headers);
            }
        }

        public static IResponse<TResponse> Post<TRequest, TResponse>(string uriString, TRequest request)
        {
            var uri = new Uri(uriString);
            using (var client = GetClient(uri))
            {
                var responseTask = client.PostAsync(uri.PathAndQuery, request, new JsonMediaTypeFormatter());
                var response = responseTask.Result;

                return new Response<TResponse>(response.Content.ReadAsAsync<TResponse>().Result, response.StatusCode, response.Headers);
            }
        }

        public static IResponse<string> Delete(string uriString)
        {
            var uri = new Uri(uriString);
            using (var client = GetClient(uri))
            {
                var responseTask = client.DeleteAsync(uri.PathAndQuery);
                var response = responseTask.Result;

                return new Response<string>(string.Empty, response.StatusCode, response.Headers);
            }
        }

        private static HttpClient GetClient(Uri uri)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(string.Format("{0}://{1}:{2}", uri.Scheme, uri.Host, uri.Port))
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
