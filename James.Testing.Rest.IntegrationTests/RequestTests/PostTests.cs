using System;
using James.Testing.Rest.IntegrationTests.Models;
using Nancy;
using Nancy.ModelBinding;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace James.Testing.Rest.IntegrationTests.RequestTests
{
    [TestFixture]
    public class given_resource_when_posting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void Init()
        {
            var person = new Person {FirstName = "Tammy", LastName = "Meinershagen"};
            Request
                .Post<Person, Guid>(GetUriString(PostModule.Resource), person);
        }

        [Test]
        public void given_resource_when_posting_should_return_created_status()
        {
            Request
                .CurrentResponse<Guid>()
                .Verify(r => r.StatusCode == HttpStatusCode.Created);
        }

        [Test]
        public void given_resource_when_posting_should_return_location_header()
        {
            Request
                .CurrentResponse<Guid>()
                .Verify(r => r.Headers.Location.PathAndQuery.Contains("People"));
        }

        [Test]
        public void given_resource_when_posting_should_return_content_in_body()
        {
            Request
                .CurrentResponse<Guid>()
                .Verify(r => r.Body != Guid.Empty);
        }
    }

    [TestFixture]
    public class given_resource_with_bad_request_when_posting : HostTestFixture
    {
        [TestFixtureSetUp]
        public void Init()
        {
            var person = new Person {FirstName = "Todd", LastName = "Meinershagen"};
            Request
                .Post<Person, Guid>(GetUriString(PostModule.Resource), person);
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
                .Post<Person, Guid, Error>(GetUriString(PostModule.Resource),
                    new Person {FirstName = "Todd", LastName = "Meinershagen"})
                .Verify(r => r.Body == Guid.Empty)
                .Verify(r => r.StatusCode == HttpStatusCode.BadRequest)
                .Verify(r => r.Error.Message == "People with first name 'Todd' are not allowed.");
        }
    }

    public class PostModule : NancyModule
    {
        public const string Resource = "People";

        public PostModule()
        {
            Post[Resource] = _ =>
            {
                var model = this.Bind<Person>();

                if (model.FirstName == "Tammy")
                {
                    var personId = Guid.NewGuid();
                    var location = string.Format("{0}://{1}/{2}/{3}", Request.Url.Scheme, Request.Url.HostName, Resource,
                        personId);
                    return Negotiate
                        .WithModel(Guid.NewGuid())
                        .WithStatusCode(Nancy.HttpStatusCode.Created)
                        .WithHeader("Location", location);
                }

                return Negotiate
                    .WithStatusCode(Nancy.HttpStatusCode.BadRequest)
                    .WithModel(new Error {Message = string.Format("People with first name '{0}' are not allowed.", model.FirstName)});
                
            };
        }
    }
}
