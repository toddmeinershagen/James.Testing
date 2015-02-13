using System;
using System.Linq;
using FluentAssertions;
using Nancy;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace James.Testing.Rest.IntegrationTests.RequestTests
{
    [TestFixture]
    public class given_basic_request_and_accepted_response_when_deleting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Request
                .WithUri(GetUriString(DeleteModule.PeopleResource) + "/" + DeleteModule.AcceptedId)
                .Delete();
        }

        [Test]
        public void should_return_status()
        {
            Request
                .CurrentResponse()
                .Verify(r => r.StatusCode == HttpStatusCode.NoContent);
        }

        [Test]
        public void should_not_send_headers()
        {
            DeleteModule.HeaderSent.Should().BeFalse();
        }

        [Test]
        public void should_not_send_query()
        {
            DeleteModule.QuerySent.Should().BeFalse();
        }

        [Test]
        public void should_return_body_as_empty_string()
        {
            Request
                .CurrentResponse()
                .Verify(r => string.IsNullOrEmpty(r.Body));
        }

        public void should_return_execution_time()
        {
            Request
                .CurrentResponse()
                .VerifyThat(r => r.ExecutionTime.Should().BeGreaterThan(TimeSpan.Zero));
        }
    }

    [TestFixture]
    public class given_request_with_headers_and_accepted_response_when_deleting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Request
                .WithUri(GetUriString(DeleteModule.PeopleResource) + "/" + DeleteModule.AcceptedId)
                .WithHeaders(new{x_requested_with = "XMLHttpRequest"})
                .Delete();
        }

        [Test]
        public void should_return_status()
        {
            Request
                .CurrentResponse()
                .Verify(r => r.StatusCode == HttpStatusCode.NoContent);
        }

        [Test]
        public void should_send_header()
        {
            DeleteModule.HeaderSent.Should().BeTrue();
        }

        [Test]
        public void should_not_send_query()
        {
            DeleteModule.QuerySent.Should().BeFalse();
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
    public class given_request_with_query_and_accepted_response_when_deleting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Request
                .WithUri(GetUriString(DeleteModule.PeopleResource) + "/" + DeleteModule.AcceptedId)
                .WithQuery(new {FirstName = "Todd"})
                .Delete();
        }

        [Test]
        public void should_return_status()
        {
            Request
                .CurrentResponse()
                .Verify(r => r.StatusCode == HttpStatusCode.NoContent);
        }

        [Test]
        public void should_not_send_header()
        {
            DeleteModule.HeaderSent.Should().BeFalse();
        }

        [Test]
        public void should_send_query()
        {
            DeleteModule.QuerySent.Should().BeTrue();
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
                .WithUri(GetUriString(DeleteModule.PeopleResource) + "/" + Guid.NewGuid())
                .Delete();
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
            var body = Request
                .CurrentResponse()
                .Verify(r => r.Body.message == "This is the message.");
        }
    }

    public class DeleteModule : NancyModule
    {
        public const string PeopleResource = "People";
        public static Guid AcceptedId = Guid.NewGuid();
        public static bool HeaderSent = false;
        public static bool QuerySent = false;

        public DeleteModule()
        {
            Delete[PeopleResource + "/{Id}"] = _ =>
            {
                if (_.Id == AcceptedId)
                {
                    HeaderSent = Request.Headers["x-requested-with"].FirstOrDefault() != null;
                    QuerySent = Request.Query.FirstName != null;

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
