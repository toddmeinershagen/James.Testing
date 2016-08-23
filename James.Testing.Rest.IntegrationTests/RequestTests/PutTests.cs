using System;
using System.Linq;
using James.Testing.Rest.IntegrationTests.Models;
using Nancy;
using Nancy.ModelBinding;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;
using FluentAssertions;

namespace James.Testing.Rest.IntegrationTests.RequestTests
{
    [TestFixture]
    public class given_basic_request_when_putting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void Init()
        {
            var person = new Person { FirstName = "Tammy", LastName = "Meinershagen" };
            Request
                .WithUri(GetUriString(PutModule.Resource))
                .Put<Person, Guid>(person);
        }

        [Test]
        public void should_return_created_status()
        {
            Request
                .CurrentResponse<Guid>()
                .Verify(r => r.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public void should_not_send_header()
        {
            PutModule.HeaderSent.Should().BeFalse();
        }

        [Test]
        public void should_not_send_query()
        {
            PutModule.QuerySent.Should().BeFalse();
        }

        [Test]
        public void should_return_location_header()
        {
            Request
                .CurrentResponse<Guid>()
                .Verify(r => r.Headers.Location.PathAndQuery.Contains(PutModule.Resource));
        }

        [Test]
        public void should_return_content_in_body()
        {
            Request
                .CurrentResponse<Guid>()
                .Verify(r => r.Body != Guid.Empty);
        }

        public void should_return_execution_time()
        {
            Request
                .CurrentResponse<Guid>()
                .VerifyThat(r => r.ExecutionTime.Should().BeGreaterThan(TimeSpan.Zero));
        }
    }

    [TestFixture]
    public class given_request_with_headers_when_putting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void Init()
        {
            var person = new Person { FirstName = "Tammy", LastName = "Meinershagen" };
            Request
                .WithUri(GetUriString(PutModule.Resource))
                .WithHeaders(new { x_requested_with = "XMLHttpRequest" })
                .Put<Person, Guid>(person);
        }

        [Test]
        public void should_return_created_status()
        {
            Request
                .CurrentResponse<Guid>()
                .Verify(r => r.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public void should_send_header()
        {
            PutModule.HeaderSent.Should().BeTrue();
        }

        [Test]
        public void should_not_send_query()
        {
            PutModule.QuerySent.Should().BeFalse();
        }

        [Test]
        public void should_return_location_header()
        {
            Request
                .CurrentResponse<Guid>()
                .Verify(r => r.Headers.Location.PathAndQuery.Contains(PutModule.Resource));
        }

        [Test]
        public void should_return_content_in_body()
        {
            Request
                .CurrentResponse<Guid>()
                .Verify(r => r.Body != Guid.Empty);
        }
    }

    [TestFixture]
    public class given_request_with_query_when_putting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void Init()
        {
            Request
                .WithUri(GetUriString(PutModule.Resource))
                .WithQuery(new { Id = PutModule.NoBodyId })
                .Put();
        }

        [Test]
        public void should_return_ok_status()
        {
            Request
                .CurrentResponse()
                .VerifyThat(r => r.StatusCode.Should().Be(HttpStatusCode.OK));
        }

        [Test]
        public void should_not_send_header()
        {
            PutModule.HeaderSent.Should().BeFalse();
        }

        [Test]
        public void should_send_query()
        {
            PutModule.QuerySent.Should().BeTrue();
        }

        [Test]
        public void should_return_location_header()
        {
            Request
                .CurrentResponse()
                .VerifyThat(r => r.Headers.Location.PathAndQuery.Should().Contain(PutModule.Resource));
        }

        [Test]
        public void should_return_content_in_body()
        {
            Request
                .CurrentResponse()
                .VerifyThat(r => ((object)r.Body).Should().NotBeNull());
        }
    }

    [TestFixture]
    public class given_resource_with_bad_request_when_putting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void Init()
        {
            var person = new Person { FirstName = "Todd", LastName = "Meinershagen" };
            Request
                .WithUri(GetUriString(PutModule.Resource))
                .Put<Person, Guid>(person);
        }

        [Test]
        public void given_resource_with_bad_request_when_posting_should_return_error_in_body()
        {
            Request
                .CurrentResponse<Guid>()
                .Verify(r => r.Body == Guid.Empty)
                .Verify(r => r.StatusCode == HttpStatusCode.BadRequest)
                .Verify(r => r.Error == "{\"message\":\"People with first name \\u0027Todd\\u0027 are not allowed.\"}");

            Request
                .WithUri(GetUriString(PostModule.Resource))
                .Post<Person, Guid, Error>(new Person { FirstName = "Todd", LastName = "Meinershagen" })
                .Verify(r => r.Body == Guid.Empty)
                .Verify(r => r.StatusCode == HttpStatusCode.BadRequest)
                .Verify(r => r.Error.Message == "People with first name 'Todd' are not allowed.");
        }
    }

    public class PutModule : NancyModule
    {
        public const string Resource = "People_Put";
        public static bool HeaderSent = false;
        public static bool QuerySent = false;
        public static readonly string NoBodyId = "2";

        public PutModule()
        {
            Put[Resource] = _ =>
            {
                var model = Request.Query.Id == NoBodyId ? null : this.Bind<Person>();

                if (model == null || model.FirstName == "Tammy")
                {
                    var personId = Guid.NewGuid();
                    var location = string.Format("{0}://{1}/{2}/{3}", Request.Url.Scheme, Request.Url.HostName, Resource,
                        personId);

                    HeaderSent = Request.Headers["x-requested-with"].FirstOrDefault() != null;
                    QuerySent = Request.Query.Id != null;

                    return Negotiate
                        .WithModel(personId)
                        .WithStatusCode(Nancy.HttpStatusCode.OK)
                        .WithHeader("Location", location);
                }

                return Negotiate
                    .WithStatusCode(Nancy.HttpStatusCode.BadRequest)
                    .WithModel(new Error { Message = string.Format("People with first name '{0}' are not allowed.", model.FirstName) });

            };
        }
    }
}
