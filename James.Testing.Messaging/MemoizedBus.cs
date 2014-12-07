using System;

namespace James.Testing.Messaging
{
    public class MemoizedBus : IBus
    {
        private readonly IBus _innerBus;

        public MemoizedBus(IBus innerBus)
        {
            _innerBus = innerBus;
        }

        public void Dispose()
        {
            _innerBus.Dispose();
        }

        public IResponse<TResponse> SendRequest<TRequest, TResponse>(Uri destinationAddress, TRequest message) where TRequest : class where TResponse : class
        {
            var response = _innerBus.SendRequest<TRequest, TResponse>(destinationAddress, message);
            _currentResponse = response;
            return response;
        }

        public IResponse<TResponse> SendRequest<TRequest, TResponse>(Uri destinationAddress, TRequest message, TimeSpan timeout) where TRequest : class where TResponse : class
        {
            var response = _innerBus.SendRequest<TRequest, TResponse>(destinationAddress, message, timeout);
            _currentResponse = response;
            return response;
        }

        private object _currentResponse;
 
        public IResponse<TResponse> CurrentResponse<TResponse>()
            where TResponse : class
        {
            return _currentResponse as IResponse<TResponse>;
        }
    }
}