using System;
using System.Diagnostics;
using MassTransit;
using MassTransit.BusConfigurators;

namespace James.Testing.Messaging.MassTransit
{
    public class Bus : IBus
    { 
        private IServiceBus _bus;

        public Bus(Uri receiveFromUri)
            : this(cfg => cfg.ReceiveFrom(receiveFromUri))
        {
        }

        public Bus(Action<ServiceBusConfigurator> configureBus)
            : this(ServiceBusFactory.New(configureBus))
        {
        }

        public Bus(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Dispose()
        {
            if (_bus == null) return;
            _bus.Dispose();
            _bus = null;
        }

        public IResponse<TResponse> SendRequest<TRequest, TResponse>(Uri destinationAddress, TRequest message)
            where TRequest : class
            where TResponse : class
        {
            return SendRequest<TRequest, TResponse>(destinationAddress, message, TimeSpan.FromSeconds(30));
        }

        public IResponse<TResponse> SendRequest<TRequest, TResponse>(Uri destinationAddress, TRequest message, TimeSpan timeout) 
            where TRequest : class 
            where TResponse : class
        {
            TResponse responseMessage = null;
            Exception exception = null;

            var watch = new Stopwatch();
            watch.Start();

            _bus
                .GetEndpoint(destinationAddress)
                .SendRequest(message, _bus, cfg =>
                {
                    cfg.Handle<TResponse>(r => responseMessage = r);
                    cfg.HandleFault(f => exception = new BusException(f.Messages[0]));
                    cfg.SetTimeout(timeout);
                });

            watch.Stop();

            if (exception == null)
                return new Response<TResponse>(responseMessage, watch.Elapsed);

            throw exception;
        }

       
    }
}
