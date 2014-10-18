using System.Net;
using James.Testing.Rest.IntegrationTests.Models;
using NUnit.Framework;

namespace James.Testing.Rest.IntegrationTests.RequestTests
{
    [TestFixture]
    public class RequestPostTests : HostTestFixture
    {
        [Test]
        public void given_anonymous_object_when_posting_should_return_dynamic_body_and_error()
        {
            Request
                .WithUri(GetUriString(PostModule.Resource))
                .Post(new {FirstName = "Tom", LastName = "Meinershagen"})
                .Verify(x => x.StatusCode == HttpStatusCode.BadRequest)
                .Verify(x => x.Body == null)
                .Verify(x => x.Error.message == "People with first name 'Tom' are not allowed.");
        }
    }
}
