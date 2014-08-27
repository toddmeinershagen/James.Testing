using System.Threading;

namespace James.Testing.Rest
{
    public class Request
    {
        private readonly string _uriString;
        private object _headers;
        private DynamicDictionary _query;

        private Request(string uriString)
        {
            _uriString = uriString;
        }

        public static Request WithUri(string uriString)
        {
            return new Request(uriString);
        }

        public Request WithHeaders(dynamic headers)
        {
            _headers = headers;
            return this;
        }

        public Request WithQuery(dynamic query)
        {
            _query = DynamicDictionary.FromObject(query);
            return this;
        }

        public Request WithQueryValue(string name, string value)
        {
            if (_query == null)
                _query = new DynamicDictionary();
            _query.Add(name, value);
            return this;
        }

        public IResponse<dynamic, dynamic> Get()
        {
            return Execute(new GetAsDynamic<dynamic>(_uriString, _headers, _query));
        }
 
        public IResponse<TResponse, dynamic> Get<TResponse>()
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

        public IResponse<dynamic, dynamic> Post(dynamic body)
        {
            return Post<object, object, object>(body);
        }

        public IResponse<dynamic, dynamic> Delete()
        {
            return Execute(new DeleteRequest<object, object>(_uriString, _headers, _query));
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

        public static IResponse<TResponse, dynamic> CurrentResponse<TResponse>()
        {
            return CurrentResponse<TResponse, string>();
        }

        public static IResponse<dynamic, dynamic> CurrentResponse()
        {
            return CurrentResponse<dynamic, dynamic>();
        }
    }
}
