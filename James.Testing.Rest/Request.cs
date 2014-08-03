using System.Data.OleDb;
using System.Globalization;
using System.Threading;

namespace James.Testing.Rest
{
    public class Request
    {
        private readonly string _uriString;
        private object _headers;
        private object _query;

        private Request(string uriString)
        {
            _uriString = uriString;
        }

        public static Request WithUri(string uriString)
        {
            return new Request(uriString);
        }

        public Request WithHeaders(object headers)
        {
            _headers = headers;
            return this;
        }

        public Request WithQuery(object query)
        {
            _query = query;
            return this;
        }

        public IResponse<dynamic, dynamic> Get()
        {
            return Execute(new GetAsDynamic<dynamic>(_uriString, _headers, _query));
        }
 
        public IResponse<TResponse, string> Get<TResponse>()
        {
            return Execute(new GetRequest<TResponse, string>(_uriString, _headers, _query));
        }

        public IResponse<byte[], string> GetAsBytes()
        {
            return Execute(new GetAsBytes<string>(_uriString, _headers, _query));
        }

        public IResponse<TResponse, TError> Post<TBody, TResponse, TError>(TBody body)
        {
            return Execute(new PostRequest<TBody, TResponse, TError>(_uriString, body, _headers, _query));
        }

        public IResponse<TResponse, string> Post<TBody, TResponse>(TBody body)
        {
            return Post<TBody, TResponse, string>(body);
        }

        public IResponse<string, string> Delete()
        {
            return Execute(new DeleteRequest<string>(_uriString, _headers, _query));
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
