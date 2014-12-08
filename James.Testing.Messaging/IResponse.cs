using System;

namespace James.Testing.Messaging
{
    public interface IResponse<out TResponse> 
        where TResponse : class
    {
        TResponse Message { get; }

        TimeSpan Elapsed { get; }
    }

    public static class IResponseExtensions
    {
        public static MemoizedBus Bus<TResponse>(this IResponse<TResponse> response)
            where TResponse : class
        {
            var busResponse = response as BusResponse<TResponse>;
            return busResponse == null ? null : busResponse.Bus;
        }
    }
}