using System.Reflection;
using System.Threading;

namespace James.Testing.Rest
{
    public class Request
    {
        public static IResponse<TResponse, string> Get<TResponse>(string uriString)
        {
            return Get<TResponse>(uriString, null);
        }

        public static IResponse<TResponse, string> Get<TResponse>(string uriString, object headers)
        {
            return Execute(new GetRequest<TResponse, string>(uriString, headers));
        }

        public static IResponse<dynamic, string> GetAsDynamic(string uriString)
        {
            return GetAsDynamic(uriString, null);
        }

        public static IResponse<dynamic, string> GetAsDynamic(string uriString, object headers)
        {
            return Execute(new GetAsDynamicRequest<string>(uriString, headers));
        }

        public static IResponse<byte[], string> GetAsBytes(string uriString)
        {
            return Execute(new GetAsBytesRequest<string>(uriString, null));
        }

        public static IResponse<TResponse, string> Post<TRequest, TResponse>(string uriString, TRequest request)
        {
            return Post<TRequest, TResponse, string>(uriString, request);
        }

        public static IResponse<TResponse, TError> Post<TRequest, TResponse, TError>(string uriString, TRequest request)
        {
            return Execute(new PostRequest<TRequest, TResponse, TError>(uriString, request, null));
        }

        public static IResponse<string, string> Delete(string uriString)
        {
            return Execute(new DeleteRequest<string>(uriString));
        }

        private static IResponse<TResponse, TError> Execute<TResponse, TError>(IRequest<TResponse, TError> request)
        {
            var response = request.Execute();
            _currentResponse.Value = response;
            return response;
        }

        private static readonly ThreadLocal<object> _currentResponse = new ThreadLocal<object>();
 
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
