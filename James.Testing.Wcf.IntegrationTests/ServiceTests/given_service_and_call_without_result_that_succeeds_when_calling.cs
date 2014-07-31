using System;
using NUnit.Framework;

namespace James.Testing.Wcf.IntegrationTests.ServiceTests
{
    [TestFixture]
    public class given_service_and_call_without_result_that_succeeds_when_calling : HostFixture<PersonService>
    {
        private readonly Guid _id = Guid.NewGuid();

        [TestFixtureSetUp]
        public void Init()
        {
            Service<IPersonService>
                .Call(x => x.MarkPersonAsFavorite(_id));
        }

        [Test]
        public void should_return_result_properly()
        {
            Service<IPersonService>
                .CurrentResponse()
                .Verify(r => r.Result == Result.Empty);
        }

        [Test]
        public void should_return_fault_as_null()
        {
            Service<IPersonService>
                .CurrentResponse()
                .Verify(r => r.Fault == null);
        }
    }
}