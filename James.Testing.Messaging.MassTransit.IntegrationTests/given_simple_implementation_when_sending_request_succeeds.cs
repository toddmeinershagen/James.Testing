using System;
using System.Threading;
using FluentAssertions;
using James.Testing.Messaging.MassTransit.IntegrationTests.Infrastructure;
using MassTransit;
using NUnit.Framework;

namespace James.Testing.Messaging.MassTransit.IntegrationTests
{
    [TestFixture]
    public class given_simple_implementation_when_sending_request_succeeds : LoopbackLocalAndRemoteTestFixture
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
        public void should_return_response_message()
        {
            var id = Guid.NewGuid();
            
            Messaging
                .With(new BusEnvironment(LocalBus))
                .SendRequest<Input, Output>(RemoteUri, new Input{Id = id})
                .VerifyThat(r => r.Message.Id.Should().Be(id));
             
            //Messaging
            //    .With<BusEnvironment>()
            //    .CurrentResponse()
            //    .VerifyThat(r => r.Message.Id.Should().Be(id));
        }

        [Test]
        public void should_return_response_elapsed()
        {
            var id = Guid.NewGuid();

            Messaging
                .With(new BusEnvironment(LocalBus))
                .SendRequest<Input, Output>(RemoteUri, new Input { Id = id })
                .VerifyThat(r => r.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(2), 500));
        }

        public class Input
        {
            public Guid Id { get; set; }
        }

        public class Output
        {
            public Guid Id { get; set; }
        }
    }
}
