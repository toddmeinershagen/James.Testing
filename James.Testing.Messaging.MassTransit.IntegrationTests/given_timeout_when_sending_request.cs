using System;
using System.Threading;
using FluentAssertions;
using James.Testing.Messaging.MassTransit.IntegrationTests.Infrastructure;
using MassTransit;
using MassTransit.Exceptions;
using NUnit.Framework;

namespace James.Testing.Messaging.MassTransit.IntegrationTests
{
    [TestFixture]
    public class given_timeout_when_sending_request : LoopbackLocalAndRemoteTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            RemoteBus.SubscribeContextHandler<Input>(Respond);
        }

        public void Respond(IConsumeContext<Input> context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            context.Respond(new Output { Id = context.Message.Id });
        }

        [Test]
        public void should_throw_timeout_exception()
        {
            Action action = ()=> 
                Messaging
                .With(new BusEnvironment(LocalBus))
                .SendRequest<Input, Output>(RemoteUri, new Input { Id = Guid.NewGuid() }, TimeSpan.FromSeconds(0));

            action
                .ShouldThrow<RequestTimeoutException>();
        }
    }
}
