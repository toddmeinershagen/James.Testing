using System;
using System.ServiceModel;
using NUnit.Framework;

namespace James.Testing.Wcf.IntegrationTests.ServiceTests
{
    [TestFixture]
    public class given_service_and_call_without_result_that_fails_when_calling : HostFixture<PersonService>
    {
        private readonly Guid _id = Guid.Empty;

        [TestFixtureSetUp]
        public void Init()
        {
            Service<IPersonService>
                .Call<FaultException<GeneralFault>>(x => x.MarkPersonAsFavorite(_id));
        }

        [Test]
        public void should_return_result_properly()
        {
            Service<IPersonService>
                .CurrentResponse<Result, FaultException<GeneralFault>>()
                .Verify(r => r.Result == Result.Empty);
        }

        [Test]
        public void should_return_fault_properly()
        {
            Service<IPersonService>
                .CurrentResponse<Result, FaultException<GeneralFault>>()
                .Verify(r => r.Fault.Detail.Message == "This is the message.")
                .Verify(r => r.Fault.Detail.StatusDescription == "BadRequest")
                .Verify(r => r.Fault.Detail.StatusCode == 400);
        }
    }
}