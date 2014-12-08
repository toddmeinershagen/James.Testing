using System;

namespace James.Testing.Messaging
{
    public class MemoizedBus
    {
        private readonly IBus _innerBus;
        private object _currentResponse;

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
            _currentResponse = AsBusResponse(response);
            
            return _currentResponse as BusResponse<TResponse>;
        }

        public IResponse<TResponse> SendRequest<TRequest, TResponse>(Uri destinationAddress, TRequest message, TimeSpan timeout) where TRequest : class where TResponse : class
        {
            var response = _innerBus.SendRequest<TRequest, TResponse>(destinationAddress, message, timeout);
            _currentResponse = AsBusResponse(response);

            return _currentResponse as BusResponse<TResponse>;
        }
 
        public IResponse<TResponse> CurrentResponse<TResponse>()
            where TResponse : class
        {
            return _currentResponse as BusResponse<TResponse>;
        }

        private BusResponse<TResponse> AsBusResponse<TResponse>(IResponse<TResponse> response)
            where TResponse : class
        {
            return new BusResponse<TResponse>(this, response);
        }
    }
}