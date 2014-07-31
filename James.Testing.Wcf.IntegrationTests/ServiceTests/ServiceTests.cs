using System;
using System.ServiceModel;
using System.Threading.Tasks;
using NUnit.Framework;

namespace James.Testing.Wcf.IntegrationTests.ServiceTests
{
    [TestFixture]
    public class ServiceTests : HostFixture<PersonService>
    {
        [Test]
        public void given_service_without_a_call_when_getting_current_response_should_return_null()
        {
            Action action = () =>
            Service<IPersonService>
                .CurrentResponse<GetPersonResult, FaultException<GeneralFault>>()
                .Verify(r => r == null);

            Parallel.Invoke(action);
        }
    }
}
