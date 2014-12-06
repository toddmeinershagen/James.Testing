using MassTransit;

namespace James.Testing.Messaging.MassTransit.IntegrationTests.Infrastructure
{
    public class BusEnvironment : IBusEnvironment
    {
        private readonly IBus _bus;

        public BusEnvironment(IServiceBus bus)
        {
            _bus = new Bus(bus);
        }

        public IBus CreateBus()
        {
            return _bus;
        }
    }
}
