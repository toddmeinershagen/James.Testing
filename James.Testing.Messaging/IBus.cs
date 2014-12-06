using System;

namespace James.Testing.Messaging
{
    public interface IBus : IDisposable
    {
        IResponse<TResponse> SendRequest<TRequest, TResponse>(Uri destinationAddress, TRequest message)
            where TRequest : class
            where TResponse : class;

        IResponse<TResponse> SendRequest<TRequest, TResponse>(Uri destinationAddress, TRequest message, TimeSpan timeout)
            where TRequest : class
            where TResponse : class;
    }
}
