using System;

namespace James.Testing.Messaging
{
    public interface IResponse<out TResponse> 
        where TResponse : class
    {
        TResponse Message { get; }

        TimeSpan Elapsed { get; }
    }
}