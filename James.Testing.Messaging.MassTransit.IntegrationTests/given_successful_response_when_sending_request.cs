using System;
using System.Threading;
using FluentAssertions;
using James.Testing.Messaging.MassTransit.IntegrationTests.Infrastructure;
using MassTransit;
using NUnit.Framework;

namespace James.Testing.Messaging.MassTransit.IntegrationTests
{
    [TestFixture]
    public class given_successful_response_when_sending_request : LoopbackLocalAndRemoteTestFixture
    {
        private Guid _id;

        [SetUp]
        public void SetUp()
        {
            RemoteBus.SubscribeContextHandler<Input>(Respond);

            _id = Guid.NewGuid();

            Messaging
                .With(new BusEnvironment(LocalBus))
                .SendRequest<Input, Output>(RemoteUri, new Input {Id = _id});
        }

        [TearDown]
        public void TearDown()
        {
            Messaging
                .Bus
                .Dispose();
        }

        public void Respond(IConsumeContext<Input> context)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            context.Respond(new Output { Id = context.Message.Id });
        }

        [Test]
        public void should_return_response_message()
        {
            Messaging
                .CurrentResponse<Output>()
                .VerifyThat(r => r.Message.Should().NotBeNull())
                .VerifyThat(r => r.Message.ShouldBeEquivalentTo(new {Id = _id}));
        }

        [Test]
        public void should_return_response_elapsed()
        {
            var id = Guid.NewGuid();

            Messaging
                .CurrentResponse<Output>()
                .VerifyThat(r => r.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(2), 500));
        }
    }
}
