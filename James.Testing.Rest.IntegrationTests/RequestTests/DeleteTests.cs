using System;
using Nancy;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace James.Testing.Rest.IntegrationTests.RequestTests
{
    [TestFixture]
    public class given_resource_and_accepted_response_when_deleting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Request
                .Delete(GetUriString(DeleteModule.PeopleResource) + "/" + DeleteModule.AcceptedId);
        }

        [Test]
        public void should_return_status()
        {
            Request
                .CurrentResponse()
                .Verify(r => r.StatusCode == HttpStatusCode.NoContent);
        }

        [Test]
        public void should_return_body_as_empty_string()
        {
            Request
                .CurrentResponse()
                .Verify(r => string.IsNullOrEmpty(r.Body));
        }
    }

    [TestFixture]
    public class given_resource_and_ok_response_when_deleting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Request
                .Delete(GetUriString(DeleteModule.PeopleResource) + "/" + Guid.NewGuid());
        }

        [Test]
        public void should_return_status()
        {
            Request
                .CurrentResponse()
                .Verify(r => r.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public void should_return_body_as_empty_string()
        {
            Request
                .CurrentResponse()
                .Verify(r => string.IsNullOrEmpty(r.Body));
        }
    }

    public class DeleteModule : NancyModule
    {
        public const string PeopleResource = "People";
        public static Guid AcceptedId = Guid.NewGuid();

        public DeleteModule()
        {
            Delete[PeopleResource + "/{Id}"] = _ =>
            {
                if (_.Id == AcceptedId)
                {
                    return Negotiate
                        .WithStatusCode(Nancy.HttpStatusCode.NoContent);
                }

                return Negotiate
                    .WithModel(new DeleteStatus {Message = "This is the message."})
                    .WithStatusCode(Nancy.HttpStatusCode.OK);
            };
        }
    }

    public class DeleteStatus
    {
        public string Message { get; set; }
    }
}
