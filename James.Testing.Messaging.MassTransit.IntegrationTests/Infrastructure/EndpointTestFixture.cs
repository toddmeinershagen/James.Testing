using System;
using MassTransit;
using MassTransit.Configurators;
using MassTransit.EndpointConfigurators;
using MassTransit.Exceptions;
using MassTransit.Saga;
using MassTransit.Transports;
using NUnit.Framework;

namespace James.Testing.Messaging.MassTransit.IntegrationTests.Infrastructure
{
    public abstract class EndpointTestFixture<TTransportFactory>
        where TTransportFactory : class, ITransportFactory, new()
    {
        [SetUp]
        public void Setup()
        {
            if (_endpointFactoryConfigurator != null)
            {
                ConfigurationResult result = ConfigurationResultImpl.CompileResults(_endpointFactoryConfigurator.Validate());

                try
                {
                    EndpointFactory = _endpointFactoryConfigurator.CreateEndpointFactory();
                    _endpointCache = new EndpointCache(EndpointFactory);
                    EndpointCache = new EndpointCacheProxy(_endpointCache);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(result, "An exception was thrown during endpoint cache creation", ex);
                }
            }

            ServiceBusFactory.ConfigureDefaultSettings(x =>
            {
                x.SetEndpointCache(EndpointCache);
                x.SetConcurrentConsumerLimit(4);
                x.SetReceiveTimeout(TimeSpan.FromMilliseconds(50));
                x.EnableAutoStart();
                x.DisablePerformanceCounters();
            });

            EstablishContext();
        }

        [TearDown]
        public void Teardown()
        {
            TeardownContext();

            _endpointCache.Clear();
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            if (EndpointCache != null)
            {
                EndpointCache.Dispose();
                EndpointCache = null;
            }

            ServiceBusFactory.ConfigureDefaultSettings(x => x.SetEndpointCache(null));
        }

        readonly EndpointFactoryConfiguratorImpl _endpointFactoryConfigurator;
        EndpointCache _endpointCache;

        protected EndpointTestFixture()
        {
            var defaultSettings = new EndpointFactoryDefaultSettings();

            _endpointFactoryConfigurator = new EndpointFactoryConfiguratorImpl(defaultSettings);
            _endpointFactoryConfigurator.AddTransportFactory<TTransportFactory>();
            _endpointFactoryConfigurator.SetPurgeOnStartup(true);
        }

        protected void AddTransport<T>()
            where T : class, ITransportFactory, new()
        {
            _endpointFactoryConfigurator.AddTransportFactory<T>();
        }

        protected IEndpointFactory EndpointFactory { get; private set; }
        protected IEndpointCache EndpointCache { get; set; }

        protected virtual void EstablishContext()
        {
        }

        protected virtual void TeardownContext()
        {
        }

        protected void ConfigureEndpointFactory(Action<EndpointFactoryConfigurator> configure)
        {
            if (_endpointFactoryConfigurator == null)
                throw new ConfigurationException("The endpoint factory configurator has already been executed.");

            configure(_endpointFactoryConfigurator);
        }

        protected static InMemorySagaRepository<TSaga> SetupSagaRepository<TSaga>()
            where TSaga : class, ISaga
        {
            var sagaRepository = new InMemorySagaRepository<TSaga>();

            return sagaRepository;
        }
    }
}
