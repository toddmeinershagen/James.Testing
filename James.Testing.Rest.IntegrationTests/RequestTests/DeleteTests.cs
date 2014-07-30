using System;
using Nancy;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace James.Testing.Rest.IntegrationTests.RequestTests
{
    [TestFixture]
    public class given_resource_when_deleting : HostTestFixture
    {
        [Test]
        public void should_return_status()
        {
            Request
                .Delete(GetUriString(DeleteModule.PeopleResource) + "/" + Guid.NewGuid())
                .Verify(r => r.StatusCode == HttpStatusCode.NoContent);
        }

        [Test]
        public void given_resource_when_deleting_should_return_empty_body()
        {
            Request
                .Delete(GetUriString(DeleteModule.PeopleResource) + "/" + Guid.NewGuid())
                .Verify(r => string.IsNullOrEmpty(r.Body));
        }

    }

    public class DeleteModule : NancyModule
    {
        public const string PeopleResource = "People";

        public DeleteModule()
        {
            Delete[PeopleResource + "/{Id}"] = _ => 
                Negotiate
                .WithStatusCode(Nancy.HttpStatusCode.NoContent);
        }
    }
}
