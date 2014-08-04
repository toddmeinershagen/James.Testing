using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace James.Testing.Rest
{
    public class DynamicDictionary : DynamicObject
    {
        private readonly Dictionary<string, object> _dictionary;

        public DynamicDictionary(object value)
            : this(new Dictionary<string, object>())
        {
            foreach (var property in value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                _dictionary[property.Name] = property.GetValue(value, null);
            }
        }

        public DynamicDictionary()
            : this(new Dictionary<string, object>())
        {}

        public DynamicDictionary(Dictionary<string, object> dictionary)
        {
            this._dictionary = dictionary;
        }

        public static DynamicDictionary FromObject(object value)
        {
            return new DynamicDictionary(value);
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;

            return true;
        }

        public List<string> GetMemberNames()
        {
            return _dictionary.Keys.ToList();
        }

        public object Get(string name)
        {
            return _dictionary[name];
        }

        public void Add(string name, string value)
        {
            _dictionary[name] = value;
        }
    }

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

        public static IResponse<dynamic, dynamic> CurrentResponse()
        {
            return CurrentResponse<dynamic, dynamic>();
        }
    }
}
