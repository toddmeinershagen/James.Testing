using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using James.Testing.Rest.IntegrationTests.Models;
using Nancy;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace James.Testing.Rest.IntegrationTests.RequestTests
{
    [TestFixture]
    public class GetTests : HostTestFixture
    {
        [Test]
        public void given_uri_for_existing_resource_when_getting_should_return_resource()
        {
            Request
                .WithUri(GetUriString(GetModule.PeopleResource))
                .Get<IList<Person>>()
                .Verify("Body != null", r => r.Body != null)
                .Verify("Count == 2", r => r.Body.Count == 2)
                .Verify("Item 1, FirstName = Todd", r => r.Body[0].FirstName == "Todd")
                .Verify("Item 1, LastName = Meinershagen", r => r.Body[0].LastName == "Meinershagen")
                .Verify("Item 2, FirstName = Brian", r => r.Body[1].FirstName == "Brian")
                .Verify("Item 2, LastName = Ellis", r => r.Body[1].LastName == "Ellis")
                .Verify("StatusCode == OK", r => r.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public void given_uri_for_non_existing_resource_when_getting_should_return_with_not_found_status()
        {
            Request
                .WithUri(GetUriString("DoesNotExist"))
                .Get<object>()
                .Verify("Body == null", r => r.Body == null)
                .VerifyThat(r => r.StatusCode).Is(HttpStatusCode.NotFound);
        }

        [Test]
        public void given_uri_for_existing_resource_with_headers_when_getting_should_return_resource_properly()
        {
            Request
                .WithUri(GetUriString(GetModule.HeadersResource))
                .WithHeaders(new {Id = "1;1", Test = "verify", x_medassets_auth = "test"})
                .Get<Dictionary<string, string>>()
                .VerifyThat(r => r.StatusCode).Is(HttpStatusCode.OK)
                .Verify(r => r.Body["id"] == "1;1")
                .Verify(r => r.Body["test"] == "verify")
                .Verify(r => r.Body["x-medassets-auth"] == "test");
        }

        [Test]
        public void given_uri_for_existing_resource_with_query_when_getting_should_return_resource_properly()
        {
            Request
                .WithUri(GetUriString(GetModule.QueryResource))
                .WithQueryValue("FirstName", "Todd")
                .WithQueryValue("LastName", "Meinershagen")
                .Get<Person>()
                .VerifyThat(r => r.StatusCode).Is(HttpStatusCode.OK)
                .Verify(r => r.Body.FirstName == "Todd")
                .Verify(r => r.Body.LastName == "Meinershagen");
        }

        [Test]
        public void given_uri_for_existing_resource_when_getting_as_dynamic_should_return_resource()
        {
            Request
                .WithUri(GetUriString(GetModule.DynamicResource))
                .Get()
                .Verify(x => x.Body.firstName == "Todd")
                .Verify(x => x.Body.lastName == "Meinershagen");
        }

        [Test]
        public void given_uri_for_existing_resource_with_headers_when_getting_as_dynamic_should_return_resource()
        {
            Request
                .WithUri(GetUriString(GetModule.HeadersResource))
                .WithHeaders(new {Id = "1", FirstName = "Tammy", LastName = "Bennett"})
                .Get()
                .Verify(r => r.Body.id == "1")
                .Verify(r => r.Body.firstName == "Tammy")
                .Verify(r => r.Body.lastName == "Bennett");
        }

        [Test]
        public void given_uri_for_non_existing_resource_when_getting_as_dynamic_should_return_with_bad_request_status_with_dynamic_error_object()
        {
            Request
                .WithUri(GetUriString(GetModule.DynamicResource))
                .WithQuery(new {Id = GetModule.BadRequestId})
                .Get()
                .Verify("Body == null", r => r.Body == null)
                .Verify(r => r.Error.message == "This is the message.")
                .VerifyThat(r => r.StatusCode).Is(HttpStatusCode.BadRequest);
        }

        [Test]
        public void given_uri_for_non_existing_resource_with_non_matching_response_media_type_when_getting_as_dynamic_should_return_with_bad_request_status_with_error_message()
        {
            Request
                .WithUri(GetUriString(GetModule.DynamicResource))
                .WithQuery(new { Id = GetModule.UnsupportedMediaExceptionId })
                .Get()
                .Verify("Body == null", r => r.Body == null)
                .Verify(r => r.Error == "This is the error message.")
                .VerifyThat(r => r.StatusCode).Is(HttpStatusCode.BadRequest);
        }
    }

    [TestFixture]
    public class given_existing_resource_when_getting_as_bytes : HostTestFixture
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Request
                .WithUri(GetUriString(GetModule.DocumentsResource) + "/" + Guid.NewGuid())
                .GetAsBytes();
        }

        [Test]
        public void should_return_bytes()
        {
            Request
                .CurrentResponse<byte[]>()
                .Verify(r => r.Body.Length > 0);
        }

        [Test]
        public void should_be_ok_status()
        {
            Request
                .CurrentResponse<byte[]>()
                .Verify(r => r.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public void should_return_headers()
        {
            Request
                .CurrentResponse<byte[]>()
                .Verify(r => r.Headers.Any());
        }
    }

    public class GetModule : NancyModule
    {
        public const string PeopleResource = "People";
        public const string DocumentsResource = "Documents";
        public const string HeadersResource = "Headers";
        public const string DynamicResource = "Dynamic";
        public const string QueryResource = "Query";
        public static Guid BadRequestId = Guid.NewGuid();
        public static Guid UnsupportedMediaExceptionId = Guid.NewGuid();

        private readonly Person[] _people = new[]
        {
            new Person { Id = Guid.NewGuid(), FirstName = "Todd", LastName = "Meinershagen" },
            new Person { Id = Guid.NewGuid(), FirstName = "Brian", LastName = "Ellis" }
        };

        public GetModule()
        {
            Get[PeopleResource] = _ => Negotiate
                .WithModel(_people)
                .WithStatusCode(Nancy.HttpStatusCode.OK);

            Get[DynamicResource] = _ =>
            {
                if (Request.Query.Id == BadRequestId)
                {
                    return Negotiate
                        .WithModel(new Error {Message = "This is the message."})
                        .WithStatusCode(Nancy.HttpStatusCode.BadRequest);
                }

                if (Request.Query.Id == UnsupportedMediaExceptionId)
                {
                    return Response
                        .AsText("This is the error message.")
                        .WithStatusCode(Nancy.HttpStatusCode.BadRequest);
                }

                return Negotiate
                    .WithModel(new {FirstName = "Todd", LastName = "Meinershagen"});
            };

            Get[HeadersResource] = _ =>
            {
                var headers = Request.Headers.ToDictionary(header => header.Key, header => header.Value.First());

                return Negotiate
                    .WithModel(headers);
            };

            Get[QueryResource] = _ =>
            {
                var query = Request.Query;
                return Negotiate
                    .WithModel(new Person{FirstName = query.FirstName, LastName = query.LastName});
            };

            Get[DocumentsResource + "/{Id}"] = _ =>
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, "SampleEstimate.pdf");
                return Response.FromByteArray(File.ReadAllBytes(filePath), "application/pdf");
            };
        }
    }
}
