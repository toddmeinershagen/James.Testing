using System;
using System.ServiceModel;
using NUnit.Framework;

namespace James.Testing.Wcf.IntegrationTests.ServiceTests
{
    [TestFixture]
    public class given_service_and_call_with_result_that_fails_when_calling : HostFixture<PersonService>
    {
        private readonly Guid _id = Guid.Empty;

        [TestFixtureSetUp]
        public void Init()
        {
            Service<IPersonService>
                .Call<GetPersonResult, FaultException<GeneralFault>>(x => x.GetPerson(new GetPersonRequest {Id = _id}));
        }

        [Test]
        public void should_return_result_as_null()
        {
            Service<IPersonService>
                .CurrentResponse<GetPersonResult, FaultException<GeneralFault>>()
                .Verify(r => r.Result == null);
        }

        [Test]
        public void should_return_fault_properly()
        {
            Service<IPersonService>
                .CurrentResponse<GetPersonResult, FaultException<GeneralFault>>()
                .Verify(r => r.Fault.Detail.Message == "This is the message.")
                .Verify(r => r.Fault.Detail.StatusDescription == "PersonNotFound")
                .Verify(r => r.Fault.Detail.StatusCode == 404);
        }
    }
}