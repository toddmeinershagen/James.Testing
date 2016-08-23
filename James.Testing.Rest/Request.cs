using System;
using System.Diagnostics;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using JetBrains.Annotations;

namespace James.Testing.Rest
{
    public class Request
    {
        internal readonly string UriString;
        private object _headers;
        private DynamicDictionary _query;
        private MediaTypeFormatter _formatter;

        private Request(string uriString)
        {
            UriString = uriString;
        }

        public static Request WithUri(string uriString)
        {
            new Uri(uriString);
            return new Request(uriString);
        }

        [StringFormatMethod("uriFormat")]
        public static Request WithUri(string uriFormat, params object[] args)
        {
            return WithUri(string.Format(uriFormat, args));
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

        public Request WithFormatter(MediaTypeFormatter formatter)
        {
            _formatter = formatter;
            return this;
        }

        public IResponse<dynamic, dynamic> Get()
        {
            return Get<dynamic, dynamic>();
        }
 
        public IResponse<TResponse, dynamic> Get<TResponse>()
        {
            return Get<TResponse, dynamic>();
        }

        public IResponse<TResponse, TError> Get<TResponse, TError>()
        {
            return Execute(new GetRequest<TResponse, TError>(UriString, _headers, _query, _formatter));
        }

        public IResponse<dynamic, dynamic> Post()
        {
            return Post(null);
        }

        public IResponse<dynamic, dynamic> Post(dynamic body)
        {
            return Post<object, object, object>(body);
        }

        public IResponse<TResponse, string> Post<TBody, TResponse>(TBody body)
        {
            return Post<TBody, TResponse, string>(body);
        }

        public IResponse<TResponse, TError> Post<TBody, TResponse, TError>(TBody body)
        {
            return Execute(new PostRequest<TBody, TResponse, TError>(UriString, body, _headers, _query));
        }

        public IResponse<dynamic, dynamic> Put()
        {
            return Put(null);
        }

        public IResponse<dynamic, dynamic> Put(dynamic body)
        {
            return Put<object, object, object>(body);
        }

        public IResponse<TResponse, string> Put<TBody, TResponse>(TBody body)
        {
            return Put<TBody, TResponse, string>(body);
        }

        public IResponse<TResponse, TError> Put<TBody, TResponse, TError>(TBody body)
        {
            return Execute(new PutRequest<TBody, TResponse, TError>(UriString, body, _headers, _query));
        }

        public IResponse<dynamic, dynamic> Delete()
        {
            return Execute(new DeleteRequest<object, object>(UriString, _headers, _query));
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
            return CurrentResponse<TResponse, dynamic>();
        }

        public static IResponse<dynamic, dynamic> CurrentResponse()
        {
            return CurrentResponse<dynamic, dynamic>();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var uriWithQuery = new Uri(UriString).With(_query);
            builder.Append(uriWithQuery.AbsoluteUri);

            var headerValues = _headers.GetHeaderValues();
            foreach (var key in headerValues.AllKeys)
            {
                builder.AppendFormat("{0}{1}: {2}", Environment.NewLine, key, headerValues[key]);
            }

            return builder.ToString();
        }
    }
}
