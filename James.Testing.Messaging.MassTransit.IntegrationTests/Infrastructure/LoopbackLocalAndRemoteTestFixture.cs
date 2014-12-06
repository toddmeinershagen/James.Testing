using System;
using MassTransit;
using MassTransit.BusConfigurators;
using MassTransit.Subscriptions.Coordinator;
using MassTransit.Transports.Loopback;

namespace James.Testing.Messaging.MassTransit.IntegrationTests.Infrastructure
{
    public abstract class LoopbackLocalAndRemoteTestFixture :
        EndpointTestFixture<LoopbackTransportFactory>
    {
        public IServiceBus LocalBus { get; protected set; }
        public IServiceBus RemoteBus { get; protected set; }

        protected override void EstablishContext()
        {
            base.EstablishContext();

            LocalBus = ServiceBusFactory.New(ConfigureLocalBus);
            RemoteBus = ServiceBusFactory.New(ConfigureRemoteBus);

            _localLoopback.SetTargetCoordinator(_remoteLoopback.Router);
            _remoteLoopback.SetTargetCoordinator(_localLoopback.Router);
        }

        SubscriptionLoopback _localLoopback;
        SubscriptionLoopback _remoteLoopback;
        protected Uri LocalUri;
        protected Uri RemoteUri;

        protected virtual void ConfigureLocalBus(ServiceBusConfigurator configurator)
        {
            LocalUri = new Uri("loopback://localhost/mt_client");
            configurator.ReceiveFrom(LocalUri);
            configurator.AddSubscriptionObserver((bus, coordinator) =>
            {
                _localLoopback = new SubscriptionLoopback(bus, coordinator);
                return _localLoopback;
            });
        }

        protected virtual void ConfigureRemoteBus(ServiceBusConfigurator configurator)
        {
            RemoteUri = new Uri("loopback://localhost/mt_server");
            configurator.ReceiveFrom(RemoteUri);
            configurator.AddSubscriptionObserver((bus, coordinator) =>
            {
                _remoteLoopback = new SubscriptionLoopback(bus, coordinator);
                return _remoteLoopback;
            });
        }

        protected override void TeardownContext()
        {
            if (RemoteBus != null)
            {
                RemoteBus.Dispose();
                RemoteBus = null;
            }

            if (LocalBus != null)
            {
                LocalBus.Dispose();
                LocalBus = null;
            }

            base.TeardownContext();
        }
    }
}
