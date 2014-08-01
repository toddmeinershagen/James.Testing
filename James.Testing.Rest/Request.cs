using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;

namespace James.Testing.Rest
{
    public class Request
    {
        private static readonly ThreadLocal<object> _currentResponse = new ThreadLocal<object>();
 
        public static IResponse<TResponse, string> Get<TResponse>(string uriString)
        {
            Func<Uri, HttpClient, IResponse<TResponse, string>> function = (uri, client) =>
            {
                var responseTask = client.GetAsync(uri.PathAndQuery);
                var response = responseTask.Result;

                return new Response<TResponse, string>(response, r => r.Content.ReadAsAsync<TResponse>().Result);
            };

            return GetResponse(uriString, function);
        }

        public static IResponse<byte[], string> GetAsBytes(string uriString)
        {
            Func<Uri, HttpClient, IResponse<byte[], string>> function = (uri, client) =>
            {
                var responseTask = client.GetAsync(uri.PathAndQuery);
                var response = responseTask.Result;

                return new Response<byte[], string>(response, r => r.Content.ReadAsByteArrayAsync().Result);
            };

            return GetResponse(uriString, function);
        }

        public static IResponse<TResponse, string> Post<TRequest, TResponse>(string uriString, TRequest request)
        {
            return Post<TRequest, TResponse, string>(uriString, request);
        }

        public static IResponse<TResponse, TError> Post<TRequest, TResponse, TError>(string uriString, TRequest request)
        {
            Func<Uri, HttpClient, IResponse<TResponse, TError>> function = (uri, client) =>
            {
                var responseTask = client.PostAsync(uri.PathAndQuery, request, new JsonMediaTypeFormatter());
                var response = responseTask.Result;

                return new Response<TResponse, TError>(response, r => r.Content.ReadAsAsync<TResponse>().Result);
            };

            return GetResponse(uriString, function);
        }

        private static IResponse<TResponse, TError> GetResponse<TResponse, TError>(string uriString, Func<Uri, HttpClient, IResponse<TResponse, TError>> getResponse)
        {
            var uri = new Uri(uriString);
            IResponse<TResponse, TError> response;

            using (var client = GetClient(uri))
            {
                response = getResponse(uri, client);
                _currentResponse.Value = response;
            }

            return response;
        }

        public static IResponse<string, string> Delete(string uriString)
        {
            Func<Uri, HttpClient, IResponse<string, string>> function = (uri, client) =>
            {
                var responseTask = client.DeleteAsync(uri.PathAndQuery);
                var response = responseTask.Result;

                return new Response<string, string>(response, r => string.Empty);
            };

            return GetResponse(uriString, function);
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

        public static IResponse<TResponse, TError> CurrentResponse<TResponse, TError>()
        {
            return _currentResponse.IsValueCreated ? _currentResponse.Value as IResponse<TResponse, TError> : null;
        }

        public static IResponse<TResponse, string> CurrentResponse<TResponse>()
        {
            return CurrentResponse<TResponse, string>();
        }

        public static IResponse<string, string> CurrentResponse()
        {
            return CurrentResponse<string, string>();
        }
    }
}
