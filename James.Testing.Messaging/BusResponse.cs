using System;

namespace James.Testing.Messaging
{
    internal class BusResponse<TResponse> : IResponse<TResponse>
        where TResponse : class

    {
        private readonly MemoizedBus _bus;
        private readonly IResponse<TResponse> _response;

        public BusResponse(MemoizedBus bus, IResponse<TResponse> response)
        {
            _bus = bus;
            _response = response;
        }

        public TResponse Message
        {
            get { return _response.Message; }
        }

        public TimeSpan Elapsed
        {
            get { return _response.Elapsed; }
        }

        public MemoizedBus Bus
        {
            get { return _bus; }
        }
    }
}