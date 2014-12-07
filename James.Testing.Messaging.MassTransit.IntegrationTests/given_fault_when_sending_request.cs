using System;
using FluentAssertions;
using James.Testing.Messaging.MassTransit.IntegrationTests.Infrastructure;
using MassTransit;
using NUnit.Framework;

namespace James.Testing.Messaging.MassTransit.IntegrationTests
{
    [TestFixture]
    public class given_fault_when_sending_request : LoopbackLocalAndRemoteTestFixture
    {
        private readonly ArgumentException _expectedException = new ArgumentException();

        [SetUp]
        public void SetUp()
        {
            RemoteBus.SubscribeContextHandler<Input>(Respond);
        }

        public void Respond(IConsumeContext<Input> context)
        {
            throw _expectedException;
        }

        [Test]
        public void should_throw_timeout_exception()
        {
            Action action = () =>
                Messaging
                    .With(new BusEnvironment(LocalBus))
                    .SendRequest<Input, Output>(RemoteUri, new Input {Id = Guid.NewGuid()});

            action
                .ShouldThrow<BusException>()
                .WithMessage(_expectedException.Message);
        }
    }
}
