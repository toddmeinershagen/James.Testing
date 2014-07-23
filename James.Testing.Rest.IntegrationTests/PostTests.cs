using System;
using Nancy;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace James.Testing.Rest.IntegrationTests
{
    [TestFixture]
    public class PostTests : HostTestFixture
    {
        [Test]
        public void given_resource_when_posting_should_return_created_status()
        {
            Request
                .Post<Person, Guid>(GetUriString(PostModule.Resource), new Person { FirstName = "Reeti", LastName = "Das" })
                .Verify(r => r.StatusCode == HttpStatusCode.Created);
        }

        [Test]
        public void given_resource_when_posting_should_return_location_header()
        {
            Request
                .Post<Person, Guid>(GetUriString(PostModule.Resource), new Person { FirstName = "Reeti", LastName = "Das" })
                .Verify(r => r.Headers.Location.PathAndQuery.Contains("People"));
        }

        [Test]
        public void given_resource_when_posting_should_return_content_in_body()
        {
            Request
                .Post<Person, Guid>(GetUriString(PostModule.Resource), new Person { FirstName = "Reeti", LastName = "Das" })
                .Verify(r => r.Body != Guid.Empty);
        }
    }

    public class PostModule : NancyModule
    {
        public const string Resource = "People";

        public PostModule()
        {

            Post["/People"] = _ =>
            {
                var personId = Guid.NewGuid();
                var location = string.Format("{0}://{1}/People/{2}", Request.Url.Scheme, Request.Url.HostName, personId);
                return Negotiate
                    .WithModel(Guid.NewGuid())
                    .WithStatusCode(Nancy.HttpStatusCode.Created)
                    .WithHeader("Location", location);
            };
        }
    }
}
