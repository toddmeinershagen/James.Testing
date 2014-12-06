using System;

namespace James.Testing.Messaging.MassTransit
{
    public class Response<TResponse> : IResponse<TResponse> 
        where TResponse : class
    {
        public Response(TResponse message, TimeSpan elapsed)
        {
            Message = message;
            Elapsed = elapsed;
        }

        public TResponse Message { get; private set; }
        public TimeSpan Elapsed { get; private set; }
    }
}