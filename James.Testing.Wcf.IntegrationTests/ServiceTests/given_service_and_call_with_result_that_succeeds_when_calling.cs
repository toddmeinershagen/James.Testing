using System;
using NUnit.Framework;

namespace James.Testing.Wcf.IntegrationTests.ServiceTests
{
    [TestFixture]
    public class given_service_and_call_with_result_that_succeeds_when_calling : HostFixture<PersonService>
    {
        private readonly Guid _id = Guid.NewGuid();

        [TestFixtureSetUp]
        public void Init()
        {
            Service<IPersonService>
                .Call(x => x.GetPerson(new GetPersonRequest {Id = _id}));
        }

        [Test]
        public void should_return_result_properly()
        {
            Service<IPersonService>
                .CurrentResponse<GetPersonResult>()
                .Verify(r => r.Result.Person.Id == _id)
                .Verify(r => r.Result.Person.FirstName == "Todd")
                .Verify(r => r.Result.Person.LastName == "Meinershagen");
        }

        [Test]
        public void should_return_fault_as_null()
        {
            Service<IPersonService>
                .CurrentResponse<GetPersonResult>()
                .Verify(r => r.Fault == null);
        }
    }
}